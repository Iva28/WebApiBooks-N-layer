using BooksAppCore.Models;
using BooksAppCore.Repositories;
using Dapper.FastCrud;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace BooksInfrastructure.Repositories
{
    public class BookRepository : IBookRepository
    {
        //private MyDbContext context;
       
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

        public async Task<List<Book>> Select()
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                 var res = await dbConnection.FindAsync<Book>();
                 return res.ToList();
            }
            //return context.Books.ToList(); 
        }

        public async Task<Book> Select(int id)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                return await dbConnection.GetAsync<Book>(new Book { Id = id });
            }
            //return context.Books.Find(id);
        }

        public async Task Insert(Book book)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                await dbConnection.InsertAsync(book);
            }
            //context.Books.Add(book);
            //return context.SaveChanges();
        }

        public async Task<bool> Update(Book book)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                return await dbConnection.UpdateAsync<Book>(book);
            }

            //Book editBook = context.Books.Find(book.Id);
            //editBook.Title = book.Title;
            //editBook.Year = book.Year;
            //return context.SaveChanges();
        }

        public async Task<bool> Delete(Book book)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                return await dbConnection.DeleteAsync<Book>(book);
            }
            //context.Books.Remove(book);
            //return context.SaveChanges();
        }

        public async Task<bool> Delete(int id)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                return await dbConnection.DeleteAsync(new Book { Id = id });
            }
            //context.Books.Remove(context.Books.Find(id));
            //return context.SaveChanges();
        }
    }
}
