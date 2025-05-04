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
    public class JustificationService : IJustificationService
    {
        private readonly IRepositoryUoW _repositoryUoW;

        public JustificationService(IRepositoryUoW repositoryUoW)
        {
            _repositoryUoW = repositoryUoW;
        }
        public async Task<Result<JustificationEntity>> Add(JustificationEntity justificationEntity)
        {
            using var transaction = _repositoryUoW.BeginTransaction();
            try
            {
                var isValid = await IsValidAttendanceJustificationRequest(justificationEntity);
                if (!isValid.Success)
                {
                    Log.Error(LogMessages.InvalidAttendanceRecordInputs());
                    return Result<JustificationEntity>.Error(isValid.Message);
                }

                Result<JustificationEntity> result;

                if (justificationEntity.PointId == null || justificationEntity.PointId == 0)
                {
                    result = await HandleAbsenceJustification(justificationEntity);
                }
                else
                {
                    result = await HandleIncompletePointJustification(justificationEntity);
                }

                if (result.Success)
                {
                    await _repositoryUoW.SaveAsync();
                    await transaction.CommitAsync();
                }
                else
                {
                    transaction.Rollback();
                }

                return result;
            }
            catch (Exception ex)
            {
                Log.Error(LogMessages.AddingAttendanceJustificationError(ex));
                transaction.Rollback();
                throw new InvalidOperationException("Error to add a new Justification", ex);
            }
            finally
            {
                Log.Error(LogMessages.AddingAttendanceJustificationSuccess());
                transaction.Dispose();
            }
        }

        public async Task<List<JustificationEntity>> Get()
        {
            using var transaction = _repositoryUoW.BeginTransaction();
            try
            {
                List<JustificationEntity> attendanceJustificationEntities = await _repositoryUoW.AttendanceJustificationRepository.Get();
                _repositoryUoW.Commit();
                return attendanceJustificationEntities;
            }
            catch (Exception ex)
            {
                Log.Error(LogMessages.GetAllAttendanceJustificationError(ex));
                transaction.Rollback();
                throw new InvalidOperationException("Error to loading the list Justification");
            }
            finally
            {
                Log.Error(LogMessages.GetAllAttendanceJustificationSuccess());
                transaction.Dispose();
            }
        }

        private async Task<Result<JustificationEntity>> IsValidAttendanceJustificationRequest(JustificationEntity attendanceJustificationEntity)
        {
            var requestValidator = await new JustificationRequestValidator().ValidateAsync(attendanceJustificationEntity);
            if (!requestValidator.IsValid)
            {
                string errorMessage = string.Join(" ", requestValidator.Errors.Select(e => e.ErrorMessage));
                errorMessage = errorMessage.Replace(Environment.NewLine, "");
                return Result<JustificationEntity>.Error(errorMessage);
            }

            return Result<JustificationEntity>.Ok();
        }

        private async Task<Result<JustificationEntity>> HandleAbsenceJustification(JustificationEntity justificationEntity)
        {
            var date = justificationEntity.Date.Date;
            var userId = justificationEntity.UserId;

            var isPast = date < DateTime.UtcNow.Date;
            if (!isPast)
                return Result<JustificationEntity>.Error("You can only justify absences for past dates.");

            var existingPoint = await _repositoryUoW.AttendanceRecordRepository.GetAllByUserIdAndDate(userId, date);

            if (existingPoint != null)
                return Result<JustificationEntity>.Error("A point record already exists for this date.");

            var justification = new JustificationEntity
            {
                UserId = userId,
                Date = date,
                Reason = justificationEntity.Reason,
                Status = JustificationStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            await _repositoryUoW.AttendanceJustificationRepository.Add(justification);

            return Result<JustificationEntity>.Ok("Justification saved", justification);
        }

        private async Task<Result<JustificationEntity>> HandleIncompletePointJustification(JustificationEntity justificationEntity)
        {
            var point = await _repositoryUoW.AttendanceRecordRepository.GetById(justificationEntity.PointId ?? 0);

            if (point == null)
                return Result<JustificationEntity>.Error("Point record not found.");

            var isIncomplete = point.MorningEntry == null ||
                               point.MorningExit == null ||
                               point.AfternoonEntry == null ||
                               point.AfternoonExit == null;

            if (!isIncomplete)
                return Result<JustificationEntity>.Error("This point is complete and does not require justification.");

            var justification = new JustificationEntity
            {
                UserId = point.UserId,
                Date = point.Date,
                PointId = point.Id,
                Reason = justificationEntity.Reason,
                Status = JustificationStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            await _repositoryUoW.AttendanceJustificationRepository.Add(justification);

            return Result<JustificationEntity>.Ok("Justification saved", justification);
        }
    }
}