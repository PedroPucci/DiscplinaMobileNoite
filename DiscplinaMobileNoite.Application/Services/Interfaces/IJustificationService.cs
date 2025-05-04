using DiscplinaMobileNoite.Application.ExtensionError;
using DiscplinaMobileNoite.Domain.Entity;

namespace DiscplinaMobileNoite.Application.Services.Interfaces
{
    public interface IJustificationService
    {
        Task<Result<JustificationEntity>> Add(JustificationEntity attendanceJustificationEntity);
    }
}