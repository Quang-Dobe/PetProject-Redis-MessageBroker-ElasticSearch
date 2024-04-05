using Services.First.Data;

namespace Services.First.Services.Abstraction
{
    public interface IUserServices
    {
        Task<User> GetSingleAsync(Guid id);

        Task AddSingleAsync(User user);

        Task UpdateSingleAsync(User user);

        Task DeleteSingleAsync(Guid id);
    }
}
