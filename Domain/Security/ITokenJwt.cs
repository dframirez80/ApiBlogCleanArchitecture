using Domain.Repository.Entities;

namespace Domain.Security
{
    public interface ITokenJwt
    {
        string GenerateToken(User user, int expiresMinutes);
        int ValidateToken(string token);
    }
}
