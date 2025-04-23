using DiscplinaMobileNoite.Domain.Dto;

namespace DiscplinaMobileNoite.Infrastracture.Repository.Interfaces
{
    public interface IRecoverPasswordRepository
    {
        Task<RecoverPasswordResponse> RecoverPassword(RecoverPasswordResponse recoverPasswordResponse);
    }
}