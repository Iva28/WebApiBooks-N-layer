using BooksAppCore.Models;
using Microsoft.EntityFrameworkCore;

namespace BooksInfrastructure.EF
{
    public class MyDbContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountToken> AccountTokens { get; set; }

        public MyDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>().HasData(
            new {
                Id = 1,
                Login = "user1",
                Password = "1111",
                Role = "user",
                About = "About user1"
            },
            new {
                Id = 2,
                Login = "admin1",
                Password = "1111",
                Role = "admin",
                About = "About admin1"
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
