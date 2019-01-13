using BooksAppCore.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BooksAppCore.Repositories
{
    public interface IBookRepository
    {
        //List<Book> Select();
        //Book Select(int id);
        //int Insert(Book book);
        //int Update(Book book);
        //int Delete(Book book);
        //int Delete(int id);

        Task<List<Book>> Select();
        Task<Book> Select(int id);
        Task Insert(Book book);
        Task<bool> Update(Book book);
        Task<bool> Delete(Book book);
        Task<bool> Delete(int id);
    }
}
