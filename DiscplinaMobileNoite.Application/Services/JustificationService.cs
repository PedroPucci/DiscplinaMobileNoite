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

        //public async Task<Result<JustificationEntity>> Add(JustificationEntity justificationEntity)
        //{
        //    using var transaction = _repositoryUoW.BeginTransaction();

        //    try
        //    {
        //        // 1. Verificar se a data é futura
        //        if (justificationEntity.Date > DateTime.Now.Date)
        //            return Result<JustificationEntity>.Error("A data não pode ser maior que a data atual.");

        //        // 2. Buscar pontos do usuário na data
        //        var pontosNaData = await _repositoryUoW.AttendanceRecordRepository
        //            .GetByUserIdAndDate(justificationEntity.UserId, justificationEntity.Date);

        //        PointEntity pontoPrincipal;
        //        int camposPreenchidos = 0;
        //        int totalCampos = 4 * (pontosNaData?.Count ?? 0); // 4 campos por ponto

        //        if (pontosNaData == null || !pontosNaData.Any())
        //        {
        //            // Nenhum ponto → ausência
        //            pontoPrincipal = new PointEntity
        //            {
        //                UserId = justificationEntity.UserId,
        //                Date = justificationEntity.Date.Date,
        //                MorningEntry = null,
        //                MorningExit = null,
        //                AfternoonEntry = null,
        //                AfternoonExit = null,
        //                Status = PointStatus.Absence,
        //                CreatedAt = DateTime.UtcNow
        //            };

        //            await _repositoryUoW.AttendanceRecordRepository.Add(pontoPrincipal);
        //        }
        //        else
        //        {
        //            pontoPrincipal = pontosNaData.First();

        //            foreach (var ponto in pontosNaData)
        //            {
        //                if (ponto.MorningEntry.HasValue) camposPreenchidos++;
        //                if (ponto.MorningExit.HasValue) camposPreenchidos++;
        //                if (ponto.AfternoonEntry.HasValue) camposPreenchidos++;
        //                if (ponto.AfternoonExit.HasValue) camposPreenchidos++;
        //            }

        //            if (camposPreenchidos == 0)
        //            {
        //                pontoPrincipal.Status = PointStatus.Absence;
        //            }
        //            else if (camposPreenchidos == totalCampos)
        //            {
        //                pontoPrincipal.Status = PointStatus.Completed;
        //            }
        //            else
        //            {
        //                pontoPrincipal.Status = PointStatus.Pending;
        //            }

        //            _repositoryUoW.AttendanceRecordRepository.Update(pontoPrincipal);
        //        }

        //        // 3. Criar a justificativa
        //        var justification = new JustificationEntity
        //        {
        //            UserId = justificationEntity.UserId,
        //            Date = justificationEntity.Date.Date,
        //            Reason = justificationEntity.Reason,                    
        //            CreatedAt = DateTime.UtcNow,
        //            PointId = pontoPrincipal.Id
        //        };

        //        // 4. Definir o tipo de justificativa com base na situação
        //        if (pontoPrincipal.Status == PointStatus.Absence)
        //        {
        //            pontoPrincipal.Status = PointStatus.Absence;
        //            justification.Case = (JustificationCase)JustificationStatus.Pending;
        //        }
        //        else if (pontoPrincipal.Status == PointStatus.Pending)
        //        {
        //            pontoPrincipal.Status = PointStatus.Pending;
        //            justification.Case = (JustificationCase)JustificationStatus.Pending;
        //        }

        //        justification.Status = justificationEntity.Status;
        //        justification.Case = (JustificationCase)justification.Case;

        //        // 5. Salvar justificativa
        //        await _repositoryUoW.AttendanceJustificationRepository.Add(justification);
        //        await _repositoryUoW.SaveAsync();
        //        await transaction.CommitAsync();

        //        return Result<JustificationEntity>.Ok();
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(LogMessages.AddingAttendanceJustificationError(ex));
        //        await transaction.RollbackAsync();
        //        throw new InvalidOperationException("Erro ao adicionar justificativa de ponto.", ex);
        //    }
        //    finally
        //    {
        //        Log.Information(LogMessages.AddingAttendanceJustificationSuccess());
        //        transaction.Dispose();
        //    }
        //}

        public async Task<Result<JustificationEntity>> Add(JustificationEntity justificationEntity)
        {
            using var transaction = _repositoryUoW.BeginTransaction();

            try
            {
                // 1. Verificar se a data é futura
                if (justificationEntity.Date > DateTime.Now.Date)
                    return Result<JustificationEntity>.Error("A data não pode ser maior que a data atual.");

                // 2. Buscar pontos do usuário na data
                var pontosNaData = await _repositoryUoW.AttendanceRecordRepository
                    .GetByUserIdAndDate(justificationEntity.UserId, justificationEntity.Date);

                PointEntity pontoPrincipal;
                int camposPreenchidos = 0;
                int totalCampos = 4 * (pontosNaData?.Count ?? 0); // 4 campos por ponto

                if (pontosNaData == null || !pontosNaData.Any())
                {
                    // Nenhum ponto → ausência
                    pontoPrincipal = new PointEntity
                    {
                        UserId = justificationEntity.UserId,
                        Date = DateTime.SpecifyKind(justificationEntity.Date.Date, DateTimeKind.Utc),
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
                    // Contar preenchimentos
                    foreach (var ponto in pontosNaData)
                    {
                        if (ponto.MorningEntry.HasValue) camposPreenchidos++;
                        if (ponto.MorningExit.HasValue) camposPreenchidos++;
                        if (ponto.AfternoonEntry.HasValue) camposPreenchidos++;
                        if (ponto.AfternoonExit.HasValue) camposPreenchidos++;
                    }

                    //PointStatus novoStatus;
                    //if (camposPreenchidos == 0)
                    //    novoStatus = PointStatus.Absence;
                    //else if (camposPreenchidos == totalCampos)
                    //    novoStatus = PointStatus.Completed;
                    //else
                    //    novoStatus = PointStatus.Pending;

                    //// Atualizar todos os pontos com novo status
                    //foreach (var ponto in pontosNaData)
                    //{
                    //    ponto.Status = novoStatus;
                    //    _repositoryUoW.AttendanceRecordRepository.Update(ponto);
                    //}

                    // Independente da quantidade de campos, forçar como pendente
                    foreach (var ponto in pontosNaData)
                    {
                        ponto.Status = PointStatus.Pending;
                        _repositoryUoW.AttendanceRecordRepository.Update(ponto);
                    }


                    pontoPrincipal = pontosNaData.First();
                }

                // 3. Criar a justificativa
                var justification = new JustificationEntity
                {
                    UserId = justificationEntity.UserId,
                    Date = DateTime.SpecifyKind(justificationEntity.Date.Date, DateTimeKind.Utc),
                    Reason = justificationEntity.Reason,
                    CreatedAt = DateTime.UtcNow,
                    PointId = pontoPrincipal.Id,
                    Status = justificationEntity.Status
                };

                // 4. Definir o tipo de justificativa com base no status do ponto
                justification.Case = pontoPrincipal.Status switch
                {
                    PointStatus.Absence => JustificationCase.Absence,
                    PointStatus.Pending => JustificationCase.Oblivion,
                    _ => justificationEntity.Case
                };

                // 5. Salvar justificativa
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