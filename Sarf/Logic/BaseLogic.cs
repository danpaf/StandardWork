namespace Sarf.Logic;

public abstract class BaseLogic
{
    protected readonly IServiceScopeFactory ScopeFactory;

    public BaseLogic(IServiceScopeFactory scopeFactory)
    {
        ScopeFactory = scopeFactory;
    }
}