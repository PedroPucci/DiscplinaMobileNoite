using DiscplinaMobileNoite.Application.ExtensionError;
using DiscplinaMobileNoite.Application.Services.Interfaces;
using DiscplinaMobileNoite.Domain.Dto;
using DiscplinaMobileNoite.Infrastracture.Repository.RepositoryUoW;

namespace DiscplinaMobileNoite.Application.Services
{
    public class RecoverPasswordService : IRecoverPasswordService
    {
        private readonly IRepositoryUoW _repositoryUoW;

        public RecoverPasswordService(IRepositoryUoW repositoryUoW)
        {
            _repositoryUoW = repositoryUoW;
        }

        public Task<Result<RecoverPasswordResponse>> RecoverPassword(RecoverPasswordResponse recoverPasswordResponse)
        {
            throw new NotImplementedException();
        }
    }
}