using System.Net;
using Sarf.Database;
using Sarf.Database.Models;
using Sarf.Extensions;
using Sarf.Resources;
using Sarf.Utils;
using StandardShared.Logic.Models;

namespace Sarf.Logic;

public class AuthLogic : BaseLogic
{
    #region Metadata
    #region Requests

    public class IsTokenValidRequest
    {
        public string Token { get; init; } = null!;
    }

    public class LogoutRequest
    {
        public string Token { get; init; } = null!;
        public string Ip { get; init; } = null!;
    }

    public class RefreshRequest
    {
        public string RefreshToken { get; init; } = null!;
        public string Ip { get; init; } = null!;
    }

    public class SignInRequest
    {
        public string Username { get; init; } = null!;
        public string Password { get; init; } = null!;
        public string Ip { get; init; } = null!;
    }

    public class SignUpRequest
    {
        public string Email { get; init; } = null!;
        public string Username { get; init; } = null!;
        public string Password { get; init; } = null!;
        public string? FirstName { get; init; }
        public string? LastName { get; init; }
        public string? Patronymic { get; init; }
        public int? Age { get; init; }
        public long Permissions { get; init; } = 0L;
        public int Status { get; init; } = 0;
        public string FirstIp { get; init; } = null!;
    }

    #endregion
    #region Responses

    public class SignInResult : GenericLogicResult
    {
        public string? Token { get; init; }
        public string? RefreshToken { get; init; }
    }

    public class RefreshResult : GenericLogicResult
    {
        public string? Token { get; init; }
        public string? RefreshToken { get; init; }
    }

    #endregion
    #endregion

    private readonly ApplicationContext _db;
    private readonly JwtUtils _jwtUtils;

    public AuthLogic(IServiceScopeFactory scopeFactory) : base(scopeFactory)
    {
        var scope = ScopeFactory.CreateScope();
        _db = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
        _jwtUtils = scope.ServiceProvider.GetRequiredService<JwtUtils>();
    }

    public GenericLogicResult SignIn(SignInRequest model)
    {
        var user = _db.Users.FirstOrDefault(x => x.Username == model.Username);
        if (user == null)
            return new SignInResult
            {
                Result = SarfRes.InvalidAuthCredentials,
                HttpCode = HttpStatusCode.Unauthorized,
                Status = false
            };

        if (user.Password != model.Password.GetSha512())
            return new SignInResult
            {
                Result = SarfRes.InvalidAuthCredentials,
                HttpCode = HttpStatusCode.Unauthorized,
                Status = false
            };

        var token = _jwtUtils.GenerateJwtToken(user.Uid);
        var refreshToken = _jwtUtils.GenerateRefreshToken(user.Uid, model.Ip);

        _db.RefreshTokens.Add(refreshToken);
        _db.SaveChanges();

        return new SignInResult
        {
            Token = token,
            RefreshToken = refreshToken.Token,
            Status = true
        };
    }

    public GenericLogicResult SignUp(SignUpRequest model)
    {
        var user = _db.Users.FirstOrDefault(x => x.Username == model.Username || x.Email == model.Email);
        if (user != null)
            return new FailedLogicResult
            {
                Result = SarfRes.UserAlreadyExists
            };

        if (model.Username.Length is < 3 or > 25)
            return new FailedLogicResult
            {
                Result = String.Format(SarfRes.InvalidUsernameLength, "3", "25")
            };

        if (model.Password.Length is < 7 or > 125)
            return new FailedLogicResult
            {
                Result = String.Format(SarfRes.InvalidPasswordLength, "7", "125")
            };

        if (Utils.Utils.ParseEmail(model.Email) == null)
            return new FailedLogicResult
            {
                Result = SarfRes.InvalidEmail
            };

        var result = new User
        {
            Email = model.Email,
            Username = model.Username,
            Password = model.Password.GetSha512(),
            FirstName = model.FirstName ?? String.Empty,
            LastName = model.LastName ?? String.Empty,
            Patronymic = model.Patronymic ?? String.Empty,
            Age = model.Age,
            Permissions = model.Permissions,
            Status = model.Status,
            FirstIp = model.FirstIp
        };

        try
        {
            _db.Users.Add(result);
            _db.SaveChanges();
        }
        catch
        {
            return new FailedLogicResult
            {
                Result = SarfRes.SomethingWentWrong,
                HttpCode = HttpStatusCode.InternalServerError
            };
        }

        return new SuccessLogicResult();
    }

    public RefreshResult Refresh(RefreshRequest model)
    {
        var token = _db.RefreshTokens.FirstOrDefault(x => x.Token == model.RefreshToken);
        if (token == null)
            return new RefreshResult
            {
                Result = SarfRes.InvalidRefreshToken,
                HttpCode = HttpStatusCode.BadRequest,
                Status = false
            };

        if (token.ExpiresAt < DateTime.Now)
            return new RefreshResult
            {
                Result = SarfRes.TokenWasExpired,
                HttpCode = HttpStatusCode.BadRequest,
                Status = false
            };

        if (token.RevokedAt.HasValue)
            return new RefreshResult
            {
                Result = SarfRes.TokenAlreadyUsed,
                HttpCode = HttpStatusCode.BadRequest,
                Status = false
            };


        var jToken = _jwtUtils.GenerateJwtToken(token.UserUid);
        var refreshToken = _jwtUtils.GenerateRefreshToken(token.UserUid, model.Ip);

        token.RevokedAt = DateTime.Now;
        token.RevokedIp = model.Ip;

        return new RefreshResult
        {
            Status = true,
            Token = jToken,
            RefreshToken = refreshToken.Token
        };
    }

    public GenericLogicResult Logout(LogoutRequest model)
    {
        var userUid = _jwtUtils.ValidateJwtToken(model.Token);
        if (userUid == null)
            return new FailedLogicResult
            {
                Result = SarfRes.InvalidJwtToken,
                HttpCode = HttpStatusCode.Unauthorized
            };

        var user = _db.Users.FirstOrDefault(x => x.Uid == userUid.Value);
        if (user == null) // TODO: Notify
            return new FailedLogicResult
            {
                Result = SarfRes.HackedJwt,
                HttpCode = HttpStatusCode.Unauthorized
            };

        foreach (var token in user.RefreshTokens)
        {
            token.RevokedAt = DateTime.Now;
            token.RevokedIp = model.Ip;
        }

        _db.SaveChanges();

        return new SuccessLogicResult();
    }

    public GenericLogicResult IsTokenValid(IsTokenValidRequest model)
    {
        var userUid = _jwtUtils.ValidateJwtToken(model.Token);
        return userUid.HasValue
            ? new SuccessLogicResult
            {
                Result = userUid.Value
            }
            : new FailedLogicResult
            {
                HttpCode = HttpStatusCode.Unauthorized
            };
    }
}