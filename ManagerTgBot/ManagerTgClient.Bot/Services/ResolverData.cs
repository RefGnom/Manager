namespace Manager.ManagerTgClient.Bot.Services;

public class ResolverData(Type commandType, Type requestType)
{
    public Type CommandType = commandType;
    public Type CommandRequestType = requestType;
}