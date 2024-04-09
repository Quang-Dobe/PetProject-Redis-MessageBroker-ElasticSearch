using Services.Data.Core;

namespace Services.First.Repositories.Abstraction
{
    public interface IDeviceRepositories
    {
        IQueryable<Device> GetAll();

        void Add(Device entity);

        void Update(Device entity);

        void Delete(Device entity);

        void SaveChange();

        Task SaveChangeAsync();
    }
}
