using BooksAppCore.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BooksAppCore.Repositories
{
    public interface IAccountRepository
    {
        Task<Account> Get(string login, string password);
        Task<Account> GetById(int id);
        Task<AccountToken> GetToken(string refreshToken);
        Task<int> DeleteAllTokens(int id);
        Task Insert(AccountToken accountToken);
        Task<IEnumerable<Account>> Search(string str);
    }
}
