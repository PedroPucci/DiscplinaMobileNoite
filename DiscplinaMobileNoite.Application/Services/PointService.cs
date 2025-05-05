using DiscplinaMobileNoite.Application.ExtensionError;
using DiscplinaMobileNoite.Application.Services.Interfaces;
using DiscplinaMobileNoite.Domain.Entity;
using DiscplinaMobileNoite.Domain.Enum;
using DiscplinaMobileNoite.Infrastracture.Repository.RepositoryUoW;
using DiscplinaMobileNoite.Shared.Logging;
using DiscplinaMobileNoite.Shared.Validator;
using Serilog;

namespace DiscplinaMobileNoite.Application.Services
{
    public class PointService : IPointService
    {
        private readonly IRepositoryUoW _repositoryUoW;

        public PointService(IRepositoryUoW repositoryUoW)
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

                attendanceRecordEntity.CreatedAt = DateTime.UtcNow;
                attendanceRecordEntity.Date = DateTime.SpecifyKind(attendanceRecordEntity.Date, DateTimeKind.Utc);
                var pontosDoDia = await _repositoryUoW.AttendanceRecordRepository
                    .GetByUserIdAndDate(attendanceRecordEntity.UserId, attendanceRecordEntity.Date);

                int totalCampos = pontosDoDia.Count * 4;
                int camposPreenchidos = 0;

                foreach (var ponto in pontosDoDia)
                {
                    if (ponto.MorningEntry.HasValue) camposPreenchidos++;
                    if (ponto.MorningExit.HasValue) camposPreenchidos++;
                    if (ponto.AfternoonEntry.HasValue) camposPreenchidos++;
                    if (ponto.AfternoonExit.HasValue) camposPreenchidos++;
                }

                if (camposPreenchidos == 3)
                {
                    foreach (var ponto in pontosDoDia)
                    {
                        ponto.Status = PointStatus.Completed;
                        _repositoryUoW.AttendanceRecordRepository.Update(ponto);
                    }
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
                throw new InvalidOperationException("Error to add a new Point");
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
                throw new InvalidOperationException("Error to loading the list Points");
            }
            finally
            {
                Log.Error(LogMessages.GetAllAttendanceRecordSuccess());
                transaction.Dispose();
            }
        }

        public async Task<List<PointEntity>> GetByUserIdAndDate(int userId, DateTime date)
        {
            using var transaction = _repositoryUoW.BeginTransaction();
            try
            {
                var records = await _repositoryUoW.AttendanceRecordRepository
                    .GetAllByUserIdAndDate(userId, date);

                _repositoryUoW.Commit();
                return records;
            }
            catch (Exception ex)
            {
                Log.Error(LogMessages.GetAllAttendanceRecordError(ex));
                transaction.Rollback();
                throw new InvalidOperationException("Error loading points by userId and date.");
            }
            finally
            {
                Log.Error(LogMessages.GetAllAttendanceRecordSuccess());
                transaction.Dispose();
            }
        }

        public async Task<List<PointEntity>> GetByUserId(int userId)
        {
            using var transaction = _repositoryUoW.BeginTransaction();
            try
            {
                var records = await _repositoryUoW.AttendanceRecordRepository
                    .GetByUserId(userId);

                _repositoryUoW.Commit();
                return records;
            }
            catch (Exception ex)
            {
                Log.Error(LogMessages.GetAllAttendanceRecordError(ex));
                transaction.Rollback();
                throw new InvalidOperationException("Error loading points by userId.", ex);
            }
            finally
            {
                Log.Information(LogMessages.GetAllAttendanceRecordSuccess());
                transaction.Dispose();
            }
        }

        private async Task<Result<PointEntity>> IsValidAttendanceRecordRequest(PointEntity attendanceRecordEntity)
        {
            var requestValidator = await new PointRequestValidator().ValidateAsync(attendanceRecordEntity);
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