namespace Kabayan.Application.Authentication;

public interface IAccessTokenGenerator
{
    string GenerateToken(Guid userId);
}
