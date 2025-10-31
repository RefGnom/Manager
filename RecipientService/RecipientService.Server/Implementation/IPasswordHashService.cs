using Manager.Core.Common.HelperObjects.Result;
using Microsoft.AspNetCore.Identity;

namespace Manager.RecipientService.Server.Implementation;

public interface IPasswordHashService
{
    string HashPassword(string password);
    Result VerifyHashedPassword(string hashedPassword, string providedPassword);
}

public class PasswordHashService(
    IPasswordHasher<PasswordHashService> hasher
) : IPasswordHashService
{
    public string HashPassword(string password) => hasher.HashPassword(this, password);

    public Result VerifyHashedPassword(string hashedPassword, string providedPassword)
    {
        var passwordVerificationResult = hasher.VerifyHashedPassword(this, hashedPassword, providedPassword);
        return passwordVerificationResult == PasswordVerificationResult.Failed ? Result.Failure() : Result.Ok();
    }
}