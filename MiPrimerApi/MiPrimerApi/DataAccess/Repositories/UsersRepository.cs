using MiPrimerApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace MiPrimerApi.DataAccess.Repositories
{
    public interface IUsersRepository
    {
        public Task<IEnumerable<User>> GetAllUsersAsync();
        public Task<User> GetUsersAsync(int userId);
        public Task<User> CreateUserAsync(User user);
        public Task<User> UpdateUserAsync(User user);
        public Task<bool> ExistsAsync(int userId);
        public Task DeleteUserAsync(int userId);
    }

    public class UsersRepository : IUsersRepository
    {
        private readonly GalleryDbContext _dbContext;
        public UsersRepository(GalleryDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User> CreateUserAsync(User user)
        {
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
            return user;
        }

        public async Task DeleteUserAsync(int userId)
        {
            var user = await _dbContext.Users.FindAsync(userId);

            _dbContext.Users.Remove(user!);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int userId)
        {
            return await _dbContext.Users.AnyAsync(e => e.Id == userId);
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _dbContext.Users.ToListAsync();
        }

        public async Task<User> GetUsersAsync(int userId)
        {
            var user = await _dbContext.Users.FindAsync(userId);
            return user!;
        }

        public async Task<User> UpdateUserAsync(User user)
        {
            _dbContext.Entry(user).State = EntityState.Modified;
            var updated = _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();
            return updated.Entity;
        }
    }
}
