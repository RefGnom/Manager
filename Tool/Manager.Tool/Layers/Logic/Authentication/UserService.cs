using System;
using System.IO;
using System.Threading.Tasks;
using Manager.Tool.BusinessObjects;

namespace Manager.Tool.Layers.Logic.Authentication;

public class UserService : IUserService
{
    private readonly string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "manager.login");

    public LocalRecipient? FindUser()
    {
        if (!File.Exists(filePath))
        {
            return null;
        }

        var userIdString = File.ReadAllText(filePath);
        if (!Guid.TryParse(userIdString, out var userId))
        {
            throw new InvalidDataException(
                $"Не смогли определить идентификатор пользователя из файла {filePath}." +
                $" Содержимое файла \"{userIdString}\""
            );
        }

        return new LocalRecipient
        {
            Id = userId,
        };
    }

    public Task SaveUserIdAsync(Guid userId)
    {
        return File.WriteAllTextAsync(filePath, userId.ToString());
    }
}