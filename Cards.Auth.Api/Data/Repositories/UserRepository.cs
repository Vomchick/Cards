using Cards.API.Interfaces.RepositoryChilds;
using Cards.API.Models.ContextModels;
using Microsoft.EntityFrameworkCore;

namespace Cards.API.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly CardsDBContext context;

        public UserRepository(CardsDBContext context)
        {
            this.context = context;
        }

        public async Task Delete(Guid id)
        {
            var found = await context.Users.FirstOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);
            if (found != null)
            {
                context.Users.Remove(found);
                await context.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public async Task<User> Get(Guid id)
        {
            return await context.Users.FirstOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await context.Users.ToListAsync().ConfigureAwait(false);
        }

        public async Task Post(User value)
        {
            await context.Users.AddAsync(value).ConfigureAwait(false);
            await context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task Put(Guid id, User value)
        {
            var found = await context.Users.FirstOrDefaultAsync(x => x.Id==id).ConfigureAwait(false);
            if (found != null)
            {
                found.UserName = value.UserName;
                found.Password = value.Password;
                found.Role = value.Role;
                await context.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public async Task<User> Authenticate(string username, string password)
        {
            return await context.Users.SingleOrDefaultAsync(x => x.UserName == username && x.Password == password).ConfigureAwait(false);
        }
    }
}
