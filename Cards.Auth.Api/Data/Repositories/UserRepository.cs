using Cards.Auth.Api.Interfaces.RepositoryChilds;
using Cards.Auth.Api.Models.ContextModels;
using Microsoft.EntityFrameworkCore;

namespace Cards.Auth.Api.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly CardsDBContext context;
        private readonly ILogger<UserRepository> logger;

        public UserRepository(CardsDBContext context, ILogger<UserRepository> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public async Task Delete(Guid id)
        {
            try
            {
                var found = await context.Users.FirstOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);
                if (found != null)
                {
                    context.Users.Remove(found);
                    await context.SaveChangesAsync().ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
        }

        public async Task<User> Get(Guid id)
        {
            try
            {
                return await context.Users.FirstOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return null;
            }
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            try
            {
                return await context.Users.ToListAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return null;
            }
        }

        public async Task Post(User value)
        {
            try
            {
                await context.Users.AddAsync(value).ConfigureAwait(false);
                await context.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
        }

        public async Task Put(Guid id, User value)
        {
            try
            {
                var found = await context.Users.FirstOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);
                if (found != null)
                {
                    found.UserName = value.UserName;
                    found.Password = value.Password;
                    found.Role = value.Role;
                    await context.SaveChangesAsync().ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
        }

        public async Task<User> Authenticate(string username, string password)
        {
            try
            {
                return await context.Users.SingleOrDefaultAsync(x => x.UserName == username && x.Password == password).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return null;
            }
        }
    }
}
