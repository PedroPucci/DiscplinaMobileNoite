using DiscplinaMobileNoite.Application.ExtensionError;
using DiscplinaMobileNoite.Domain.Dto;

namespace DiscplinaMobileNoite.Application.Services.Interfaces
{
    public interface IRecoverPasswordService
    {
        Task<Result<RecoverPasswordResponse>> RecoverPassword(RecoverPasswordResponse recoverPasswordResponse);
    }
}