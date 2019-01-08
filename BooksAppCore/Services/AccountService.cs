using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using BooksAppCore.DTO;
using BooksAppCore.Models;

namespace BooksAppCore.Services
{
    public class AccountService : IAccountService
    {
        private AuthOptions authOptions;
        private IJwtService jwtService;

        private List<Account> accounts;
        private List<AccountToken> accountTokens;

        public AccountService(IOptions<AuthOptions> authOptions, IJwtService jwtService)
        {
            this.authOptions = authOptions.Value;
            this.jwtService = jwtService;

            accounts = new List<Account>()
            {
                new Account() { Id = 1, Login = "user1", Password = "1111", Role = "user", About = "About user1" },
                new Account() { Id = 2, Login = "admin1", Password = "1111", Role = "admin", About = "About admin1" },
            };
            accountTokens = new List<AccountToken>();
        }

        public AccountResponse SignIn(string login, string pswd)
        {
            Account acc = accounts.Find(u => u.Login == login && u.Password == pswd);
            if (acc == null) return null;
            return Authorize(acc);
        }

        public AccountResponse UpdateToken(string refreshToken)
        {
            AccountToken accToken = accountTokens.Find(x => x.RefreshToken == refreshToken);
            if (accToken == null || accToken.RefreshExpires <= DateTime.Now) return null;
            Account acc = accounts.Find(a => a.Id == accToken.AccountId);
            return Authorize(acc);
        }

        public void SignOut(int id)
        {
            accountTokens.RemoveAll(x => x.AccountId == id);
        }

        private AccountResponse Authorize(Account acc)
        {
            List<Claim> claims = new List<Claim>() {
                new Claim(ClaimsIdentity.DefaultNameClaimType, acc.Login),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, acc.Role),
                new Claim("id", acc.Id.ToString())
            };
            ClaimsIdentity identity = new ClaimsIdentity(
                claims,
                "Token",
                ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);

            

            AccountToken accountToken = new AccountToken() {
                AccountId = acc.Id,
                AccessToken = jwtService.GetJwt(identity),
                RefreshToken = Guid.NewGuid().ToString(),
                RefreshExpires = DateTime.Now.AddMinutes(authOptions.RefreshLifetime)
            };
            accountTokens.RemoveAll(x => x.AccountId == acc.Id);
            accountTokens.Add(accountToken);

            return new AccountResponse()
            {
                Login = acc.Login,
                AccessToken = accountToken.AccessToken,
                RefreshToken = accountToken.RefreshToken
            };
        }

        public Account Get(int id)
        {
            return accounts.Find(a => a.Id == id);
        }
    }
}
