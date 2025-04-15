using DiscplinaMobileNoite.Application.Services;
using DiscplinaMobileNoite.Application.Services.Interfaces;
using DiscplinaMobileNoite.Infrastracture.Repository.RepositoryUoW;

namespace DiscplinaMobileNoite.Application.UnitOfWork
{
    public class UnitOfWorkService : IUnitOfWorkService
    {
        private readonly IRepositoryUoW _repositoryUoW;

        private UserService userService;
        private AttendanceRecordService attendanceRecordService;
        private AttendanceJustificationService attendanceJustificationService;

        public UnitOfWorkService(IRepositoryUoW repositoryUoW)
        {
            _repositoryUoW = repositoryUoW;
        }

        public UserService UserService
        {
            get
            {
                if (userService is null)
                    userService = new UserService(_repositoryUoW);
                return userService;
            }
        }

        public AttendanceRecordService AttendanceRecordService
        {
            get
            {
                if (attendanceRecordService is null)
                    attendanceRecordService = new AttendanceRecordService(_repositoryUoW);
                return attendanceRecordService;
            }
        }

        public AttendanceJustificationService AttendanceJustificationService
        {
            get
            {
                if (attendanceJustificationService is null)
                    attendanceJustificationService = new AttendanceJustificationService(_repositoryUoW);
                return attendanceJustificationService;
            }
        }
    }
}