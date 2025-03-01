using System;
using System.IO;
using System.Threading.Tasks;
using ManagerService.Client.ServiceModels;

namespace Manager.Tool.Layers.Logic.Authentication;

public class UserService : IUserService
{
    private readonly string _filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "manager.login");

    public User? FindUser()
    {
        if (!File.Exists(_filePath))
        {
            return null;
        }

        var userIdString = File.ReadAllText(_filePath);
        if (!Guid.TryParse(userIdString, out var userId))
        {
            throw new InvalidDataException(
                $"Не смогли определить идентификатор пользователя из файла {_filePath}." +
                $" Содержимое файла \"{userIdString}\""
            );
        }

        return new User
        {
            Id = userId,
        };
    }

    public Task SaveUserIdAsync(Guid userId)
    {
        return File.WriteAllTextAsync(_filePath, userId.ToString());
    }
}