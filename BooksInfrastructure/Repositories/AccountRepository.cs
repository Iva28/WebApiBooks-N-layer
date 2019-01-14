using BooksAppCore.Models;
using BooksAppCore.Repositories;
using Dapper;
using Dapper.FastCrud;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace BooksInfrastructure.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly IConfiguration _config;

        public AccountRepository(IConfiguration config)
        {
            _config = config;
        }

        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("MyConnection"));
            }
        }

        public async Task<int> DeleteAllTokens(int id)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                return await dbConnection.BulkDeleteAsync<AccountToken>(statement => statement.Where($"{nameof(AccountToken.AccountId):C}={id}"));
            }
        }

        public async Task<AccountToken> GetToken(string refreshToken)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                return await dbConnection.GetAsync(new AccountToken { RefreshToken = refreshToken });
            }
        }

        public async Task Insert(AccountToken accountToken)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                await dbConnection.InsertAsync(accountToken);
            }
        }

        public async Task<Account> GetById(int id)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                return await dbConnection.GetAsync(new Account { Id = id });
            }
        }

        public async Task<Account> Get(string login, string password)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                return await dbConnection.QueryFirstAsync<Account>("SELECT * FROM Accounts WHERE Login = @Login AND Password = @Password",
                    new { Login = login, Password = password });
            }
        }

        public async Task<IEnumerable<Account>> Search(string str)
        {
            var q = $"SELECT * FROM Accounts WHERE Login LIKE '{str}%'";
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                return await dbConnection.QueryAsync<Account>(q);
            }
        }
    }
}
