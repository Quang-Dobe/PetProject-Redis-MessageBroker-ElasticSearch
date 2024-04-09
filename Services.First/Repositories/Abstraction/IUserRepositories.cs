using Services.Data.Core;

namespace Services.First.Repositories.Abstraction
{
    public interface IUserRepositories
    {
        IQueryable<User> GetAll();

        void Add(User entity);

        void Update(User entity);

        void Delete(User entity);

        void SaveChange();

        Task SaveChangeAsync();
    }
}
