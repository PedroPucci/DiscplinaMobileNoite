using DiscplinaMobileNoite.Application.Services;

namespace DiscplinaMobileNoite.Application.UnitOfWork
{
    public interface IUnitOfWorkService
    {
        UserService UserService { get; }
        PointService AttendanceRecordService { get; }
        JustificationService AttendanceJustificationService { get; }
        RecoverPasswordService RecoverPasswordService { get; }
    }
}