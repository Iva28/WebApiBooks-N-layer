using BooksAppCore.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BooksAppCore.Repositories
{
    public interface IBookRepository
    {
        Task<IEnumerable<Book>> Select();
        Task<Book> Select(int id);
        Task Insert(Book book);
        Task<bool> Update(Book book);
        Task<bool> Delete(Book book);
        Task<bool> Delete(int id);
        Task<IEnumerable<Book>> SearchBooks(string str);
        Task<IEnumerable<Author>> SearchAuthors(string str);
    }
}
