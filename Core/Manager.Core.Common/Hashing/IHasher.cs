using Manager.Core.Common.HelperObjects.Result;
using Microsoft.AspNetCore.Identity;

namespace Manager.Core.Common.Hashing;

public interface IHasher
{
    string Hash(string original);
    Result VerifyHashed(string hash, string providedOriginal);
}

public class Hasher : IHasher
{
    private readonly PasswordHasher<Hasher> hasher = new();

    public string Hash(string original) => hasher.HashPassword(this, original);

    public Result VerifyHashed(string hash, string providedOriginal)
    {
        return hasher.VerifyHashedPassword(this, hash, providedOriginal) == PasswordVerificationResult.Failed
            ? Result.Failure()
            : Result.Ok();
    }
}