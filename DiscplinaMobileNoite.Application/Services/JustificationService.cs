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
                var isValidAttendanceJustification = await IsValidAttendanceJustificationRequest(justificationEntity);

                if (!isValidAttendanceJustification.Success)
                {
                    Log.Error(LogMessages.InvalidAttendanceRecordInputs());
                    return Result<JustificationEntity>.Error(isValidAttendanceJustification.Message);
                }
                
                var justification = new JustificationEntity
                {
                    Id = justificationEntity.Id,
                    Reason = justificationEntity.Reason,
                    Status = JustificationStatus.Pending,
                };

                var result = await _repositoryUoW.AttendanceJustificationRepository.Add(justification);

                await _repositoryUoW.SaveAsync();
                await transaction.CommitAsync();

                return Result<JustificationEntity>.Ok();
            }
            catch (Exception ex)
            {
                Log.Error(LogMessages.AddingAttendanceJustificationError(ex));
                transaction.Rollback();
                throw new InvalidOperationException("Error to add a new Attendance Justification");
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
                throw new InvalidOperationException("Error to loading the list Attendance Record Justification");
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
    }
}