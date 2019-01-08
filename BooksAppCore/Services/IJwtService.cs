using System.Security.Claims;

namespace BooksAppCore.Services
{
    public interface IJwtService
    {
        string GetJwt(ClaimsIdentity identity);
    }
}
