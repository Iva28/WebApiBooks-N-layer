using System.Collections.Generic;
using System.Linq;
using BooksAppCore.Models;
using BooksAppCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BooksAppCore.Services
{
    public class BookService : IBookService
    {
        private IBookRepository bookRepository;

        public BookService(IBookRepository bookRepository)
        {
            this.bookRepository = bookRepository;

            if (bookRepository.Select().Result.Count() == 0)
            {
                var books = new List<Book>() {
                new Book() { Title = "Book 1", Year = 1990,
                    Authors = new List<Author>() { new Author() { FirstName = "Author", LastName = "One" }, new Author() { FirstName = "Author", LastName = "Two" } } },
                new Book() { Title = "Book 2", Year = 1991,
                    Authors = new List<Author>() { new Author() { FirstName = "Author", LastName = "Three" } } },
                new Book() { Title = "Book 3", Year = 1992,
                    Authors = new List<Author>() { new Author() { FirstName = "Author", LastName = "Four" }, new Author() { FirstName = "Author", LastName = "Five" } } },
                new Book() { Title = "Book 4", Year = 1993 },
                new Book() { Title = "Book 5", Year = 1994 }
                };
                foreach (var book in books)
                {
                    bookRepository.Insert(book);
                }
            }
        }

        public List<Book> Get()
        {
            return bookRepository.Select().Result.ToList();
        }

        public Book Get(int id)
        {
            return bookRepository.Select(id).Result;
        }

        public Book Insert(Book book)
        {
            bookRepository.Insert(book);         
            return book;
        }

        public Book Update(Book book)
        {
            bookRepository.Update(book);
            return book;
        }

        public void Delete(int id)
        {
            bookRepository.Delete(id);
        }

        public void Delete(Book book)
        {
            bookRepository.Delete(book);
        }

        public List<Author> GetAuthors(int id)
        {
            return bookRepository.Select(id).Result?.Authors.ToList();
        }

        public List<Book> SearchBook(string str)
        {
            return bookRepository.SearchBooks(str).Result.ToList();
        }

        public List<Author> SearchAuthor(string str)
        {
            return bookRepository.SearchAuthors(str).Result.ToList();
        }
    }
}
