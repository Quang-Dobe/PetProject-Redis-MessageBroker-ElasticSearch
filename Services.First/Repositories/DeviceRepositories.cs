using Microsoft.EntityFrameworkCore;
using Services.Data.Core;
using Services.First.Repositories.Abstraction;

namespace Services.First.Repositories
{
    public class DeviceRepositories : IDeviceRepositories
    {
        private readonly ServiceDbContext _dbContext;

        public DeviceRepositories(ServiceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<Device> GetAll()
        {
            return _dbContext.Set<Device>();
        }

        public void Add(Device entity)
        {
            _dbContext.Entry(entity).State = EntityState.Added;
        }

        public void Update(Device entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(Device entity)
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
