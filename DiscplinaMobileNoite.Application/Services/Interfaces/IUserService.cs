using DiscplinaMobileNoite.Application.ExtensionError;
using DiscplinaMobileNoite.Domain.Dto;
using DiscplinaMobileNoite.Domain.Entity;

namespace DiscplinaMobileNoite.Application.Services.Interfaces
{
    public interface IUserService
    {
        Task<Result<UserEntity>> Add(UserEntity userEntity);
        Task<Result<UserEntity>> Update(UserResponse userResponse);
    }
}