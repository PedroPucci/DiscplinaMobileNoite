using DiscplinaMobileNoite.Domain.Entity;

namespace DiscplinaMobileNoite.Infrastracture.Repository.Interfaces
{
    public interface IPointsRepository
    {
        Task<PointEntity> Add(PointEntity pointEntity);
        PointEntity Update(PointEntity pointEntity);
        Task<List<PointEntity>> Get();
        Task<PointEntity?> GetById(int? id);
        Task<List<PointEntity>> GetAllByUserIdAndDate(int userId, DateTime date);
    }
}