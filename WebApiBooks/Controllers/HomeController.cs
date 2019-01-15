using BooksAppCore.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApiBooks.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private IAccountService accountService;
        private IBookService bookService;

        public HomeController(IAccountService accountService, IBookService bookService)
        {
            this.accountService = accountService;
            this.bookService = bookService;
        }

        [HttpGet("{q}")]
        public IActionResult Search(string q)
        {
            var accounts = accountService.SearchAccount(q);
            var books = bookService.SearchBook(q);
            var authors = bookService.SearchAuthor(q);
            var res = new { accounts, books, authors };
            return new JsonResult(res);
        }
    }
}