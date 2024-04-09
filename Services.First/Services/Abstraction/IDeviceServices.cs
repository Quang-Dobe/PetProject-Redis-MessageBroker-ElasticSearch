using Services.Data.Core;

namespace Services.First.Services.Abstraction
{
    public interface IDeviceServices
    {
        Task<Device> GetSingleAsync(Guid id);

        Task AddSingleAsync(Device Device);

        Task UpdateSingleAsync(Device Device);

        Task DeleteSingleAsync(Guid id);
    }
}
