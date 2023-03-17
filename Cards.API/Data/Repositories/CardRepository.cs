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
        private readonly ILogger<CardRepository> logger;

        public CardRepository(CardsDBContext dbContext, IMemoryCache cache, ILogger<CardRepository> logger)
        {
            context = dbContext;
            this.cache = cache;
            this.logger = logger;
        }

        public async Task Delete(Guid id)
        {
            try
            {
                var found = await context.Cards.FirstOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);
                if (found != null)
                {
                    context.Cards.Remove(found);
                    await context.SaveChangesAsync().ConfigureAwait(false);
                    cache.Remove("AllCards");
                }
            }
            catch(Exception ex)
            {
                logger.LogError(ex.Message);
            }
        }

        public async Task<Card> Get(Guid id)
        {
            try
            {
                return await context.Cards.FirstOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return null;
            }
        }
        public async Task<IEnumerable<Card>> GetAll()
        {
            try
            {
                cache.TryGetValue("AllCards", out List<Card> cards);
                if (cards?.Count == 0 || cards == null)
                {
                    cards = await context.Cards.ToListAsync().ConfigureAwait(false);
                    cache.Set("AllCards", cards, new MemoryCacheEntryOptions() { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1) });
                }
                return cards;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return null;
            }
        }

        public async Task Post(Card value)
        {
            try
            {
                await context.Cards.AddAsync(value).ConfigureAwait(false);
                await context.SaveChangesAsync().ConfigureAwait(false);
                cache.Remove("AllCard");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
        }

        public async Task Put(Guid id, Card value)
        {
            try
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
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
        }
    }
}
