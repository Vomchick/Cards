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
    }
}
