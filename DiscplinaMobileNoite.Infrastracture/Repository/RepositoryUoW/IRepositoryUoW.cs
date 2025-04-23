using DiscplinaMobileNoite.Infrastracture.Repository.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace DiscplinaMobileNoite.Infrastracture.Repository.RepositoryUoW
{
    public interface IRepositoryUoW
    {
        IUserRepository UserRepository { get; }
        IPointsRepository AttendanceRecordRepository { get; }
        IJustificationRepository AttendanceJustificationRepository { get; }
        IRecoverPasswordRepository RecoverPasswordRepository { get; }

        Task SaveAsync();
        void Commit();
        IDbContextTransaction BeginTransaction();
    }
}