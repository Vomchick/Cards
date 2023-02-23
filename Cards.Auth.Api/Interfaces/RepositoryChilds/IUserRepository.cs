using Cards.Auth.Api.Models.ContextModels;

namespace Cards.Auth.Api.Interfaces.RepositoryChilds
{
    public interface IUserRepository : IBaseRepository<User>
    {
        public Task<User> Authenticate(string username, string password);
    }
}
