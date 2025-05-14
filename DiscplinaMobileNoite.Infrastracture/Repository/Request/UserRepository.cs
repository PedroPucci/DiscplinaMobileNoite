using DiscplinaMobileNoite.Domain.Entity;
using DiscplinaMobileNoite.Infrastracture.Connections;
using DiscplinaMobileNoite.Infrastracture.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DiscplinaMobileNoite.Infrastracture.Repository.Request
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<UserEntity> Add(UserEntity userEntity)
        {
            if (userEntity is null)
                throw new ArgumentNullException(nameof(userEntity), "User cannot be null");

            var result = await _context.Users.AddAsync(userEntity);
            await _context.SaveChangesAsync();

            return result.Entity;
        }

        public UserEntity Delete(UserEntity userEntity)
        {
            var response = _context.Users.Remove(userEntity);
            return response.Entity;
        }

        public async Task<UserEntity?> GetById(int? id)
        {
            return await _context.Users.FirstOrDefaultAsync(userEntity => userEntity.Id == id);
        }

        public async Task<UserEntity?> GetByEmail(string? email)
        {
            return await _context.Users.FirstOrDefaultAsync(userEntity => userEntity.Email == email);
        }

        public UserEntity Update(UserEntity userEntity)
        {
            var response = _context.Users.Update(userEntity);
            return response.Entity;
        }

        public UserEntity UpdateEmail(string? email, string? newPassword)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            user.Password = newPassword;

            _context.SaveChanges();
            return user;
        }

        public async Task<UserEntity?> GetByEmailAndPassword(string email, string password)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email && u.Password == password);
        }
    }
}