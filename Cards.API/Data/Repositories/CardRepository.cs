using Cards.API.Interfaces.RepositoryChilds;
using Cards.API.Models.ContextModels;
using Microsoft.AspNetCore.Server.IIS.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Cards.API.Data.Repositories
{
    public class CardRepository : ICardRepository 
    { 
        private readonly CardsDBContext context;
        private readonly IMemoryCache cache;

        public CardRepository(CardsDBContext dbContext, IMemoryCache cache)
        {
            context = dbContext;
            this.cache = cache;
        }

        public async Task Delete(Guid id)
        {
            var found = await context.Cards.FirstOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);
            if (found != null)
            {
                context.Cards.Remove(found);
                await context.SaveChangesAsync().ConfigureAwait(false);
                cache.Remove("AllCards");
            }
        }

        public async Task<Card> Get(Guid id)
        {
             return await context.Cards.FirstOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);
        }
        public async Task<IEnumerable<Card>> GetAll()
        {
            cache.TryGetValue("AllCards", out List<Card> cards);
            if (cards?.Count == 0 || cards == null)
            {
                cards = await context.Cards.ToListAsync().ConfigureAwait(false);
                cache.Set("AllCards", cards, new MemoryCacheEntryOptions() { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1) });
            }
            return cards;
        }

        public async Task Post(Card value)
        {
            await context.Cards.AddAsync(value).ConfigureAwait(false);
            await context.SaveChangesAsync().ConfigureAwait(false);
            cache.Remove("AllCard");
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
                cache.Remove("AllCards");
            }
        }
    }
}
