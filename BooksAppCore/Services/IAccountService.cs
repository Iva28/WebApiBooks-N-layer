using BooksAppCore.Models;
using BooksAppCore.DTO;
using System.Collections.Generic;

namespace BooksAppCore.Services
{
    public interface IAccountService
    {
        AccountResponse SignIn(string login, string pswd);
        AccountResponse UpdateToken(string refreshToken);
        void SignOut(int id);
        Account Get(int id);
        List<Account> SearchAccount(string str);
    }
}
