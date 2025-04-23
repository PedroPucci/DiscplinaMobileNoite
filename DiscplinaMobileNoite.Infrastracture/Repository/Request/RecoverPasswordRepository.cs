using DiscplinaMobileNoite.Domain.Dto;
using DiscplinaMobileNoite.Infrastracture.Connections;
using DiscplinaMobileNoite.Infrastracture.Repository.Interfaces;

namespace DiscplinaMobileNoite.Infrastracture.Repository.Request
{
    public class RecoverPasswordRepository : IRecoverPasswordRepository
    {
        private readonly DataContext _context;

        public RecoverPasswordRepository(DataContext context)
        {
            _context = context;
        }

        public Task<RecoverPasswordResponse> RecoverPassword(RecoverPasswordResponse recoverPasswordResponse)
        {
            throw new NotImplementedException();
        }
    }
}