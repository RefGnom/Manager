namespace Manager.Core.AppConfiguration.DataBase;

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