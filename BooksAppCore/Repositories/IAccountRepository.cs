using BooksAppCore.Models;

namespace BooksAppCore.Repositories
{
    public interface IAccountRepository
    {
        Account Get(string login, string password);
        Account GetById(int id);
        AccountToken GetToken(string refreshToken);
        int DeleteAllTokens (int id);
        void Insert(AccountToken accountToken);
    }
}
