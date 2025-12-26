namespace Manager.Core.IntegrationTestsCore.Configuration.Containers;

// Порядок важен, контейнеры запускаются пр возрастанию их номера
public enum ContainerType
{
    DataBase,
    Cache,
    Server,
}