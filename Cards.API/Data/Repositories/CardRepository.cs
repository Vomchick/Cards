using Cards.API.Interfaces.RepositoryChilds;
using Cards.API.Models.ContextModels;
using Microsoft.AspNetCore.Server.IIS.Core;
using Microsoft.EntityFrameworkCore;

namespace Cards.API.Data.Repositories
{
    public class CardRepository : ICardRepository 
    { 

        private readonly CardsDBContext context;

        public CardRepository(CardsDBContext dbContext)
        {
            context = dbContext;
        }

        public async Task Delete(Guid id)
        {
            var found = await context.Cards.FirstOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);
            if (found != null)
            {
                context.Cards.Remove(found);
                await context.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public async Task<Card> Get(Guid id) => await context.Cards.FirstOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);

        public async Task<IEnumerable<Card>> GetAll() => await context.Cards.ToListAsync().ConfigureAwait(false);

        public async Task Post(Card value)
        {
            await context.Cards.AddAsync(value).ConfigureAwait(false);
            await context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task Put(Guid id, Card value)
        {
            var found = await context.Cards.FirstOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);
            if (found != null)
            {
                found.ExpiryMonth = value.ExpiryMonth;
                found.ExpiryYear = value.ExpiryYear;
                found.CardNumber = value.CardNumber;
                found.CardholderName = value.CardholderName;
                found.CVC = value.CVC;
                await context.SaveChangesAsync().ConfigureAwait(false);
            }
        }
    }
}
