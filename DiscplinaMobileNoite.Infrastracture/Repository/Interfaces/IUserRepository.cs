using DiscplinaMobileNoite.Domain.Entity;

namespace DiscplinaMobileNoite.Infrastracture.Repository.Interfaces
{
    public interface IUserRepository
    {
        Task<UserEntity> Add(UserEntity userEntity);
        UserEntity Update(UserEntity userEntity);
        UserEntity UpdateEmail(string? email, string? newPassword);
        UserEntity Delete(UserEntity userEntity);
        Task<UserEntity?> GetById(int? id);
        Task<UserEntity?> GetByEmail(string? email);
    }
}