using Cards.API.Enums;
using Cards.API.Models.ContextModels;
using Microsoft.EntityFrameworkCore;

namespace Cards.API.Data
{
    public class CardsDBContext : DbContext
    {
        public CardsDBContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Card> Cards { get; set; }
        public DbSet<User> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = new Guid("06dc6118-e9af-4aac-aa8d-56fa9e2a30c5"),
                    UserName = "admin",
                    Password = "admin111",
                    Role = Role.Admin,
                },
                new User
                {
                    Id = new Guid("44fbadd1-c922-455b-adfa-bb8c502453ff"),
                    UserName = "TestUser",
                    Password = "user1234",
                    Role = Role.User,
                });
        }
    }
}
