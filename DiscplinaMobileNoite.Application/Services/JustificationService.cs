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
                if (justificationEntity.Date > DateTime.Now.Date)
                    return Result<JustificationEntity>.Error("A data não pode ser maior que a data atual.");

                var pontosNaData = await _repositoryUoW.AttendanceRecordRepository
                    .GetByUserIdAndDate(justificationEntity.UserId, justificationEntity.Date);

                PointEntity pontoPrincipal;
                int camposPreenchidos = 0;
                int totalCampos = 4 * (pontosNaData?.Count ?? 0);

                if (pontosNaData == null || !pontosNaData.Any())
                {
                    pontoPrincipal = new PointEntity
                    {
                        UserId = justificationEntity.UserId,
                        //Date = DateTime.SpecifyKind(justificationEntity.Date.Date, DateTimeKind.Utc),
                        Date = DateTime.SpecifyKind(
                            new DateTime(
                                justificationEntity.Date.Year,
                                justificationEntity.Date.Month,
                                justificationEntity.Date.Day,
                                8, 0, 0 // ← 08:00:00
                            ),
                            DateTimeKind.Utc
                        ),
                        MorningEntry = null,
                        MorningExit = null,
                        AfternoonEntry = null,
                        AfternoonExit = null,
                        Status = PointStatus.Absence,
                        CreatedAt = DateTime.UtcNow
                    };

                    await _repositoryUoW.AttendanceRecordRepository.Add(pontoPrincipal);
                }
                else
                {
                    foreach (var ponto in pontosNaData)
                    {
                        if (ponto.MorningEntry.HasValue) camposPreenchidos++;
                        if (ponto.MorningExit.HasValue) camposPreenchidos++;
                        if (ponto.AfternoonEntry.HasValue) camposPreenchidos++;
                        if (ponto.AfternoonExit.HasValue) camposPreenchidos++;
                    }
                    
                    foreach (var ponto in pontosNaData)
                    {
                        ponto.Status = PointStatus.Pending;
                        _repositoryUoW.AttendanceRecordRepository.Update(ponto);
                    }


                    pontoPrincipal = pontosNaData.First();
                }

                var justification = new JustificationEntity
                {
                    UserId = justificationEntity.UserId,
                    Date = DateTime.SpecifyKind(justificationEntity.Date.Date, DateTimeKind.Utc),
                    Reason = justificationEntity.Reason,
                    CreatedAt = DateTime.UtcNow,
                    PointId = pontoPrincipal.Id,
                    Status = justificationEntity.Status
                };

                justification.Case = pontoPrincipal.Status switch
                {
                    PointStatus.Absence => JustificationCase.Absence,
                    PointStatus.Pending => JustificationCase.Oblivion,
                    _ => justificationEntity.Case
                };

                await _repositoryUoW.AttendanceJustificationRepository.Add(justification);
                await _repositoryUoW.SaveAsync();
                await transaction.CommitAsync();

                return Result<JustificationEntity>.Ok();
            }
            catch (Exception ex)
            {
                Log.Error(LogMessages.AddingAttendanceJustificationError(ex));
                await transaction.RollbackAsync();
                throw new InvalidOperationException("Erro ao adicionar justificativa de ponto.", ex);
            }
            finally
            {
                Log.Information(LogMessages.AddingAttendanceJustificationSuccess());
                transaction.Dispose();
            }
        }
    }
}