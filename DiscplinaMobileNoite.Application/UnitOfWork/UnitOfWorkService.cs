using DiscplinaMobileNoite.Application.Services;
using DiscplinaMobileNoite.Infrastracture.Repository.RepositoryUoW;

namespace DiscplinaMobileNoite.Application.UnitOfWork
{
    public class UnitOfWorkService : IUnitOfWorkService
    {
        private readonly IRepositoryUoW _repositoryUoW;

        private UserService userService;
        private PointService attendanceRecordService;
        private JustificationService attendanceJustificationService;
        private RecoverPasswordService recoverPasswordService;

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

        public PointService AttendanceRecordService
        {
            get
            {
                if (attendanceRecordService is null)
                    attendanceRecordService = new PointService(_repositoryUoW);
                return attendanceRecordService;
            }
        }

        public JustificationService AttendanceJustificationService
        {
            get
            {
                if (attendanceJustificationService is null)
                    attendanceJustificationService = new JustificationService(_repositoryUoW);
                return attendanceJustificationService;
            }
        }

        public RecoverPasswordService RecoverPasswordService
        {
            get
            {
                if (recoverPasswordService is null)
                    recoverPasswordService = new RecoverPasswordService(_repositoryUoW);
                return recoverPasswordService;
            }
        }
    }
}