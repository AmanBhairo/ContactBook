using ContactBookApi.Models;

namespace ContactBookApi.Data.Contract
{
    public interface IVerifyPasswordHash
    {
        bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);
        string CreateToken(User user);
    }
}
