using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BooksAppCore.DTO;
using BooksAppCore.Models;
using BooksAppCore.Services;

namespace WebApiBooks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private IAccountService accountService;

        public AccountController(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        [HttpPost("SignIn")]
        public IActionResult SignIn([FromBody] AccountRequest model)
        {
            AccountResponse resp = accountService.SignIn(model.Login, model.Password);
            if (resp == null) return BadRequest();
            return new JsonResult(resp);
        }

        [HttpPost("Token")]
        public IActionResult Token([FromBody]string refreshToken)
        {
            AccountResponse resp = accountService.UpdateToken(refreshToken);
            if (resp == null) return BadRequest();
            return new JsonResult(resp);
        }

        [Authorize]
        [HttpGet("")]
        public IActionResult AccountInfo()
        {
            string id = User.Claims.FirstOrDefault(x => x.Type == "id")?.Value;
            if (String.IsNullOrEmpty(id)) return NotFound();
            Account acc = accountService.Get(Int32.Parse(id));
            return new JsonResult(acc);
        }

        [Authorize]
        [HttpPost("SignOut")]
        public IActionResult SignOut()
        {
            string id = User.Claims.FirstOrDefault(x => x.Type == "id")?.Value;
            if (String.IsNullOrEmpty(id)) return NotFound();
            accountService.SignOut(Int32.Parse(id));
            return Ok();
        }
    }
}