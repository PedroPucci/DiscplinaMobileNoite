using DiscplinaMobileNoite.Application.Services;

namespace DiscplinaMobileNoite.Application.UnitOfWork
{
    public interface IUnitOfWorkService
    {
        UserService UserService { get; }
    }
}