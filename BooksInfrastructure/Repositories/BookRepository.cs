using BooksAppCore.Models;
using BooksAppCore.Repositories;
using BooksInfrastructure.EF;
using System.Collections.Generic;
using System.Linq;

namespace BooksInfrastructure.Repositories
{
    public class BookRepository : IBookRepository
    {
        private MyDbContext context;

        public BookRepository(MyDbContext context)
        {
            this.context = context;
        }

        public List<Book> Select()
        {
            return context.Books.ToList();
        }

        public Book Select(int id)
        {
            return context.Books.Find(id);
        }

        public int Insert(Book book)
        {
            context.Books.Add(book);
            return context.SaveChanges();
        }

        public int Update(Book book)
        {
            Book editBook = context.Books.Find(book.Id);
            editBook.Title = book.Title;
            editBook.Year = book.Year;
            return context.SaveChanges();
        }

        public int Delete(Book book)
        {
            context.Books.Remove(book);
            return context.SaveChanges();
        }

        public int Delete(int id)
        {
            context.Books.Remove(context.Books.Find(id));
            return context.SaveChanges();
        }
    }
}
