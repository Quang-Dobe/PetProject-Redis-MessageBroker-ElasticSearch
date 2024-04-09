using Microsoft.EntityFrameworkCore;
using Services.Data.Core;
using Services.First.Repositories.Abstraction;
using Services.First.Services.Abstraction;

namespace Services.First.Services
{
    public class DeviceServices : IDeviceServices
    {
        private readonly IDeviceRepositories _repository;

        public DeviceServices(IDeviceRepositories repositories)
        {
            _repository = repositories;
        }

        public async Task<Device> GetSingleAsync(Guid id)
        {
            return await _repository.GetAll().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task AddSingleAsync(Device device)
        {
            _repository.Add(device);

            await _repository.SaveChangeAsync();
        }

        public async Task DeleteSingleAsync(Guid id)
        {
            var device = await GetSingleAsync(id);

            if (device != null)
            {
                _repository.Delete(device);

                await _repository.SaveChangeAsync();
            }
        }

        public async Task UpdateSingleAsync(Device device)
        {
            _repository.Update(device);

            await _repository.SaveChangeAsync();
        }
    }
}
