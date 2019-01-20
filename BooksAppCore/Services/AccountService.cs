using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using BooksAppCore.DTO;
using BooksAppCore.Models;
using BooksAppCore.Repositories;
using System.Linq;

namespace BooksAppCore.Services
{
    public class AccountService : IAccountService
    {
        private AuthOptions authOptions;
        private IJwtService jwtService;

        private IAccountRepository accountRepository;

        public AccountService(IOptions<AuthOptions> authOptions, IJwtService jwtService, IAccountRepository accountRepository)
        {
            this.authOptions = authOptions.Value;
            this.jwtService = jwtService;
            this.accountRepository = accountRepository;
        }

        public AccountResponse SignIn(string login, string pswd)
        {
            Account acc = accountRepository.Get(login, pswd).Result;
            if (acc == null) return null;
            return Authorize(acc);
        }

        public AccountResponse UpdateToken(string refreshToken)
        {
            AccountToken accToken = accountRepository.GetToken(refreshToken).Result;
            if (accToken == null || accToken.RefreshExpires <= DateTime.Now) return null;
            Account acc = accountRepository.GetById(accToken.AccountId).Result;
            return Authorize(acc);
        }

        public void SignOut(int id)
        {
            accountRepository.DeleteAllTokens(id);
        }

        private AccountResponse Authorize(Account acc)
        {
            List<Claim> claims = new List<Claim>() {
                new Claim(ClaimsIdentity.DefaultNameClaimType, acc.Login),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, acc.Role),
                new Claim("id", acc.Id.ToString())
            };
            ClaimsIdentity identity = new ClaimsIdentity(claims,"Token",ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
           
            AccountToken accountToken = new AccountToken() {
                AccountId = acc.Id,
                AccessToken = jwtService.GetJwt(identity),
                RefreshToken = Guid.NewGuid().ToString(),
                RefreshExpires = DateTime.Now.AddMinutes(authOptions.RefreshLifetime)
            };

            accountRepository.DeleteAllTokens(acc.Id);
            accountRepository.Insert(accountToken);

            return new AccountResponse() {
                Login = acc.Login,
                AccessToken = accountToken.AccessToken,
                RefreshToken = accountToken.RefreshToken
            };
        }

        public Account Get(int id)
        {
            return accountRepository.GetById(id).Result;
        }

        public List<Account> SearchAccount(string str)
        {
            return accountRepository.Search(str).Result.ToList();
        }
    }
}
