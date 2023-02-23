namespace Cards.Auth.Api.Interfaces
{
    public interface IBaseRepository<T>
    {
        public Task<IEnumerable<T>> GetAll();
        public Task<T> Get(Guid id);
        public Task Post(T value);
        public Task Put(Guid id, T value);
        public Task Delete(Guid id);
    }
}
