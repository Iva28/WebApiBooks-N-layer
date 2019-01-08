using System;

namespace BooksAppCore.Models
{
    public class AccountToken
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshExpires { get; set; }
    }
}
