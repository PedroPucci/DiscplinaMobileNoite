using DiscplinaMobileNoite.Domain.Dto;
using DiscplinaMobileNoite.Domain.Entity;

namespace DiscplinaMobileNoite.Infrastracture.Repository.Interfaces
{
    public interface IUserRepository
    {
        Task<UserEntity> Add(UserEntity userEntity);
        UserEntity Update(UserEntity userEntity);
        UserEntity Delete(UserEntity userEntity);
        Task<List<UserResponse>> Get();
        Task<UserEntity?> GetById(int? id);
    }
}