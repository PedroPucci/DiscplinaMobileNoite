using DiscplinaMobileNoite.Application.ExtensionError;
using DiscplinaMobileNoite.Domain.Entity;

namespace DiscplinaMobileNoite.Application.Services.Interfaces
{
    public interface IPointService
    {
        Task<Result<PointEntity>> Add(PointEntity attendanceRecordEntity);
        Task<List<PointEntity>> Get();
    }
}