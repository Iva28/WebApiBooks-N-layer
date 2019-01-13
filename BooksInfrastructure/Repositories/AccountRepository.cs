using BooksAppCore.Models;
using BooksAppCore.Repositories;
using Dapper;
using Dapper.FastCrud;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace BooksInfrastructure.Repositories
{
    public class AccountRepository : IAccountRepository
    {
       // private MyDbContext context;

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

        public int DeleteAllTokens(int id)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                return dbConnection.BulkDelete<AccountToken>(statement => statement.Where($"{nameof(AccountToken.AccountId):C}={id}"));
            }
        }

        public AccountToken GetToken(string refreshToken)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                return dbConnection.Get(new AccountToken { RefreshToken = refreshToken });
            }
        }

        public void Insert(AccountToken accountToken)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                dbConnection.Insert(accountToken);
            }
        }

        public Account GetById(int id)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                return dbConnection.Get(new Account { Id = id });
            }
        }

        public Account Get(string login, string password)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                return dbConnection.QueryFirst<Account>("SELECT * FROM Accounts WHERE Login = @Login AND Password = @Password",
                    new { Login = login, Password = password });
            }
        }
    }
}
