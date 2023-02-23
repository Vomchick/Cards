using Cards.API.Models.ContextModels;

namespace Cards.API.Interfaces.RepositoryChilds
{
    public interface IUserRepository : IBaseRepository<User>
    {
        public Task<User> Authenticate(string username, string password);
    }
}
