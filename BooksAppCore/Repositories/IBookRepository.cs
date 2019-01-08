using BooksAppCore.Models;
using System.Collections.Generic;

namespace BooksAppCore.Repositories
{
    public interface IBookRepository
    {
        List<Book> Select();
        Book Select(int id);
        int Insert(Book book);
        int Update(Book book);
        int Delete(Book book);
        int Delete(int id);
    }
}
