using DiscplinaMobileNoite.Application.ExtensionError;
using DiscplinaMobileNoite.Application.Services.Interfaces;
using DiscplinaMobileNoite.Domain.Entity;
using DiscplinaMobileNoite.Infrastracture.Repository.RepositoryUoW;
using DiscplinaMobileNoite.Shared.Logging;
using DiscplinaMobileNoite.Shared.Validator;
using Serilog;

namespace DiscplinaMobileNoite.Application.Services
{
    public class AttendanceRecordService : IPointService
    {
        private readonly IRepositoryUoW _repositoryUoW;

        public AttendanceRecordService(IRepositoryUoW repositoryUoW)
        {
            _repositoryUoW = repositoryUoW;
        }

        public async Task<Result<PointEntity>> Add(PointEntity attendanceRecordEntity)
        {
            using var transaction = _repositoryUoW.BeginTransaction();
            try
            {
                var isValidAttendanceRecord = await IsValidAttendanceRecordRequest(attendanceRecordEntity);

                if (!isValidAttendanceRecord.Success)
                {
                    Log.Error(LogMessages.InvalidAttendanceRecordInputs());
                    return Result<PointEntity>.Error(isValidAttendanceRecord.Message);
                }

                var result = await _repositoryUoW.AttendanceRecordRepository.Add(attendanceRecordEntity);

                await _repositoryUoW.SaveAsync();
                await transaction.CommitAsync();

                return Result<PointEntity>.Ok();
            }
            catch (Exception ex)
            {
                Log.Error(LogMessages.AddingAttendanceRecordError(ex));
                transaction.Rollback();
                throw new InvalidOperationException("Error to add a new Attendance Record");
            }
            finally
            {
                Log.Error(LogMessages.AddingAttendanceRecordSuccess());
                transaction.Dispose();
            }
        }
        public async Task<List<PointEntity>> Get()
        {
            using var transaction = _repositoryUoW.BeginTransaction();
            try
            {
                List<PointEntity> attendanceRecordEntities = await _repositoryUoW.AttendanceRecordRepository.Get();
                _repositoryUoW.Commit();
                return attendanceRecordEntities;
            }
            catch (Exception ex)
            {
                Log.Error(LogMessages.GetAllAttendanceRecordError(ex));
                transaction.Rollback();
                throw new InvalidOperationException("Error to loading the list Attendance Record");
            }
            finally
            {
                Log.Error(LogMessages.GetAllAttendanceRecordSuccess());
                transaction.Dispose();
            }
        }
        private async Task<Result<PointEntity>> IsValidAttendanceRecordRequest(PointEntity attendanceRecordEntity)
        {
            var requestValidator = await new AttendanceRecordRequestValidator().ValidateAsync(attendanceRecordEntity);
            if (!requestValidator.IsValid)
            {
                string errorMessage = string.Join(" ", requestValidator.Errors.Select(e => e.ErrorMessage));
                errorMessage = errorMessage.Replace(Environment.NewLine, "");
                return Result<PointEntity>.Error(errorMessage);
            }

            return Result<PointEntity>.Ok();
        }
    }
}