namespace Manager.Core.EFCore;

public interface IDbContextWrapperFactory
{
    DbContextWrapper Create();
}

public class DbContextWrapperFactory(
    IDbContextConfigurator dbContextConfigurator
) : IDbContextWrapperFactory
{
    public DbContextWrapper Create() => new(dbContextConfigurator);
}