using MiPrimerApi.DataAccess.Repositories;
using MiPrimerApi.Entities;
using MiPrimerApi.Exceptions;

namespace MiPrimerApi.Bussiness
{
    public interface IUsersService
    {
        public Task<IEnumerable<User>> GetAllUsersAsync();
        public Task<User> GetUserAsync(int userId);
        public Task<User> CreateUserAsync(User user);
        public Task<User> UpdateUserAsync(int id, User user);
        public Task<bool> ExistsAsync(int userId);
        public Task DeleteUserAsync(int userId);
    }

    public class UsersService : IUsersService
    {
        private readonly IUsersRepository _repository; 

        public UsersService(IUsersRepository repository)
        {
            this._repository = repository;
        }

        public async Task<User> CreateUserAsync(User user)
        {
            var created = await _repository.CreateUserAsync(user);
            return created;
        }

        public async Task DeleteUserAsync(int userId)
        {
            //Validacion previa a la eliminacion del registro?

            var user = await _repository.GetUsersAsync(userId);
            if (user == null)
            {
                //Si el usuario no existe, se puede lanzar una excepcion o simplemente retornar sin hacer nada
                throw new EntityNotFoundException($"User with ID {userId} not found");
            }

            await _repository.DeleteUserAsync(userId);
        }

        public async Task<bool> ExistsAsync(int userId)
        {
            return await _repository.ExistsAsync(userId);
        }

        public Task<IEnumerable<User>> GetAllUsersAsync()
        {
            var users = _repository.GetAllUsersAsync();
            return users;
        }

        public async Task<User> GetUserAsync(int userId)
        {
            var user = await _repository.GetUsersAsync(userId);
            if (user == null)
            {
                throw new EntityNotFoundException($"User with ID {userId} not found");
            }
            return user;
        }

        public async Task<User> UpdateUserAsync(int id, User user)
        {
            if(id != user.Id)
            {
                throw new NotSameIdException("The ID in the URL does not match the ID in the request body");
            }

            var existingUser = await _repository.GetUsersAsync(id);
            if (existingUser == null)
            {
                throw new EntityNotFoundException($"User with ID {id} not found");
            }

            var updatedUser = await _repository.UpdateUserAsync(user);
            return updatedUser;
        }
    }
}
