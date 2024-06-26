﻿using Microsoft.EntityFrameworkCore;
using Services.Data.Core;
using Services.First.Repositories.Abstraction;
using Services.First.Services.Abstraction;

namespace Services.First.Services
{
    public class UserServices : IUserServices
    {
        private readonly IUserRepositories _repository;

        public UserServices(IUserRepositories repositories)
        {
            _repository = repositories;
        }

        public async Task<User> GetSingleAsync(Guid id)
        {
            return await _repository.GetAll().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task AddSingleAsync(User user)
        {
            _repository.Add(user);

            await _repository.SaveChangeAsync();
        }

        public async Task DeleteSingleAsync(Guid id)
        {
            var user = await GetSingleAsync(id);

            if (user != null)
            {
                _repository.Delete(user);

                await _repository.SaveChangeAsync();
            }
        }

        public async Task UpdateSingleAsync(User user)
        {
            _repository.Update(user);

            await _repository.SaveChangeAsync();
        }
    }
}
