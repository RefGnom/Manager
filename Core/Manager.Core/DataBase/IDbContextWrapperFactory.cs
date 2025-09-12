namespace Manager.Core.DataBase;

internal interface IDbContextWrapperFactory
{
    DbContextWrapper Create();
}