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
    public class BookRepository : IBookRepository
    {
      
        private readonly IConfiguration _config;

        public BookRepository(IConfiguration config)
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

        public async Task<IEnumerable<Book>> Select()
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                return await dbConnection.FindAsync<Book>();
            }
        }

        public async Task<Book> Select(int id)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                return await dbConnection.GetAsync<Book>(new Book { Id = id });
            }
        }

        public async Task Insert(Book book)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                await dbConnection.InsertAsync(book);

                if (book.Authors.Count != 0) {
                    foreach (var a in book.Authors) {
                        await dbConnection.InsertAsync(a);
                    }
                }
            }
        }

        public async Task<bool> Update(Book book)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                return await dbConnection.UpdateAsync<Book>(book);
            }
        }

        public async Task<bool> Delete(Book book)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                return await dbConnection.DeleteAsync<Book>(book);
            }
        }

        public async Task<bool> Delete(int id)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                return await dbConnection.DeleteAsync(new Book { Id = id });
            }
        }

        public async Task<IEnumerable<Book>> SearchBooks(string str)
        {
            var sql = $"SELECT * FROM Books WHERE Title LIKE '{str}%'";
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                return await dbConnection.QueryAsync<Book>(sql);
            }
        }

        public async Task<IEnumerable<Author>> SearchAuthors(string str)
        {
            var sql = $"SELECT * FROM Authors WHERE FirstName LIKE '{str}%' OR LastName LIKE '{str}%'";
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                return await dbConnection.QueryAsync<Author>(sql);
            }
        }
    }
}
