using BooksAppCore.Models;
using BooksAppCore.DTO;

namespace BooksAppCore.Services
{
    public interface IAccountService
    {
        AccountResponse SignIn(string login, string pswd);
        AccountResponse UpdateToken(string refreshToken);
        void SignOut(int id);
        Account Get(int id);
    }
}
