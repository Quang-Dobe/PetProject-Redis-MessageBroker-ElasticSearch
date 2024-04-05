using Services.First.Data;
using Services.First.Services.Abstraction;

namespace Services.First.Services
{
    public class UserServices : IUserServices
    {
        public async Task<User> GetSingleAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task AddSingleAsync(User user)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteSingleAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateSingleAsync(User user)
        {
            throw new NotImplementedException();
        }
    }
}
