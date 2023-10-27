using Microsoft.EntityFrameworkCore;
using Sarf.Database.Models;
using StandardShared.Database;

namespace Sarf.Database;

public sealed class ApplicationContext : BaseDbContext
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<BlackList> BlackLists { get; set; } = null!;
    public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;

    public ApplicationContext(IConfiguration configuration) : base(configuration)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(Configuration["Database:ConnectionString"]);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(x => x.Uid);
            entity.Property(x => x.Uid).HasDefaultValueSql("uuid_generate_v4()"); // 
            entity
                .Property(x => x.Fullname)
                .HasComputedColumnSql("\"LastName\" || \"FirstName\" || \"Patronymic\"",true)
                .ValueGeneratedOnAdd();
            entity.Property(x => x.FirstName).HasDefaultValueSql("''").HasMaxLength(125);
            entity.Property(x => x.LastName).HasDefaultValueSql("''").HasMaxLength(125);
            entity.Property(x => x.Patronymic).HasDefaultValueSql("''").HasMaxLength(125);
            entity.Property(x => x.Permissions).HasDefaultValueSql("0");
            entity.Property(x => x.Status).HasDefaultValueSql("-1");
            
            entity.Property(x => x.Password).HasMaxLength(128);
            entity.Property(x => x.Email).HasMaxLength(256);
            entity.Property(x => x.Username).HasMaxLength(128);
            entity.Property(x => x.AddedDate).HasDefaultValueSql("now()");
        });

        modelBuilder.Entity<BlackList>(entity =>
        {
            entity.HasKey(x => x.Uid);
            entity.Property(x => x.Uid).HasDefaultValueSql("uuid_generate_v4()"); //
            entity.Property(x => x.Reason).HasMaxLength(1024);
            entity.Property(x => x.AddedTime).HasDefaultValueSql("now()");
        });

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(x => x.Uid);
            entity.Property(e => e.Uid).HasDefaultValueSql("uuid_generate_v4()");
            entity.HasOne(d => d.User)
                .WithMany(p => p.RefreshTokens)
                .HasForeignKey(d => d.UserUid);
        });
        
        base.OnModelCreating(modelBuilder);
    }
}