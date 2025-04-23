using DiscplinaMobileNoite.Infrastracture.Connections;
using DiscplinaMobileNoite.Infrastracture.Repository.Interfaces;
using DiscplinaMobileNoite.Infrastracture.Repository.Request;
using Microsoft.EntityFrameworkCore.Storage;
using Serilog;

namespace DiscplinaMobileNoite.Infrastracture.Repository.RepositoryUoW
{
    public class RepositoryUoW : IRepositoryUoW
    {
        private readonly DataContext _context;
        private bool _disposed = false;
        private IUserRepository? _userEntityRepository = null;
        private IPointsRepository? _attendanceRecord = null;
        private IJustificationRepository? _attendanceJustification = null;
        private IRecoverPasswordRepository? _recoverPasswordRepository = null;

        public RepositoryUoW(DataContext context)
        {
            _context = context;
        }

        public IRecoverPasswordRepository RecoverPasswordRepository
        {
            get
            {
                if (_recoverPasswordRepository is null)
                {
                    _recoverPasswordRepository = new RecoverPasswordRepository(_context);
                }
                return _recoverPasswordRepository;
            }
        }

        public IPointsRepository AttendanceRecordRepository
        {
            get
            {
                if (_attendanceRecord is null)
                {
                    _attendanceRecord = new PointsRepository(_context);
                }
                return _attendanceRecord;
            }
        }

        public IJustificationRepository AttendanceJustificationRepository
        {
            get
            {
                if (_attendanceJustification is null)
                {
                    _attendanceJustification = new JustificationRepository(_context);
                }
                return _attendanceJustification;
            }
        }

        public IUserRepository UserRepository
        {
            get
            {
                if (_userEntityRepository is null)
                {
                    _userEntityRepository = new UserRepository(_context);
                }
                return _userEntityRepository;
            }
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public IDbContextTransaction BeginTransaction()
        {
            return _context.Database.BeginTransaction();
        }

        public void Commit()
        {
            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Log.Error($"Database connection failed: {ex.Message}");
                throw new ApplicationException("Database is not available. Please check the connection.");
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _context.Dispose();
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}