using System;
using System.Threading.Tasks;
using Manager.Tool.BusinessObjects;

namespace Manager.Tool.Layers.Logic.Authentication;

public interface IUserService
{
    LocalRecipient? FindUser();
    Task SaveUserIdAsync(Guid userId);
}