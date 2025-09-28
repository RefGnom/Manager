namespace Manager.Core.EFCore;

internal interface IDbContextWrapperFactory
{
    DbContextWrapper Create();
}

internal class DbContextWrapperFactory(
    IDbContextConfigurator dbContextConfigurator
) : IDbContextWrapperFactory
{
    public DbContextWrapper Create()
    {
        return new DbContextWrapper(dbContextConfigurator);
    }
}