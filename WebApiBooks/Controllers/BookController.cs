using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BooksAppCore.Models;
using BooksInfrastructure.Services;
using BooksAppCore.Services;

namespace WebApiBooks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private IBookService bookService;

        public BookController(IBookService bookService)
        {
            this.bookService = bookService;
        }
        
        [HttpGet] // api/book
        public IActionResult Get()
        {
            return new JsonResult(bookService.Get());
        }

        [HttpGet("{id}")] // api/book/5
        public IActionResult Get(int id)
        {
            return new JsonResult(bookService.Get(id));
        }

        [Authorize]
        [HttpPost] // api/book
        public IActionResult Post([FromBody]Book book)
        {
            book = bookService.Insert(book);
            return new JsonResult(book) { StatusCode = 201 };
        }

        [Authorize]
        [HttpPut] // api/book
        public IActionResult Put([FromBody]Book book)
        {
            book = bookService.Update(book);
            return StatusCode(204);
        }

        [Authorize]
        [HttpDelete("{id}")] // api/book/2
        public IActionResult Delete(int id)
        {
            bookService.Delete(id);
            return StatusCode(204);
        }

        // api/book/1/authors
        [HttpGet("{id}/authors")]
        public IActionResult Authors(int id)
        {
            return new JsonResult(bookService.GetAuthors(id));
        }

    }
}