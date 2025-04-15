using DiscplinaMobileNoite.Domain.Entity;

namespace DiscplinaMobileNoite.Infrastracture.Repository.Interfaces
{
    public interface IJustificationRepository
    {
        Task<JustificationEntity> Add(JustificationEntity justificationEntity);
        JustificationEntity Update(JustificationEntity justificationEntity);
        Task<List<JustificationEntity>> Get();
        Task<JustificationEntity?> GetById(int? id);
    }
}