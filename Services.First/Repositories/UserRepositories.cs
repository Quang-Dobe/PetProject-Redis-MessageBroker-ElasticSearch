using Microsoft.EntityFrameworkCore;
using Services.Data.Core;
using Services.First.Repositories.Abstraction;

namespace Services.First.Repositories
{
    public class UserRepositories : IUserRepositories
    {
        private readonly ServiceDbContext _dbContext;

        public UserRepositories(ServiceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<User> GetAll()
        {
            return _dbContext.Set<User>();
        }

        public void Add(User entity)
        {
            _dbContext.Entry(entity).State = EntityState.Added;
        }

        public void Update(User entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(User entity)
        {
            _dbContext.Entry(entity).State = EntityState.Deleted;
        }

        public void SaveChange()
        {
            _dbContext.SaveChanges();
        }

        public async Task SaveChangeAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
