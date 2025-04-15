using DiscplinaMobileNoite.Domain.Entity;
using DiscplinaMobileNoite.Infrastracture.Connections;
using DiscplinaMobileNoite.Infrastracture.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DiscplinaMobileNoite.Infrastracture.Repository.Request
{
    public class JustificationRepository : IJustificationRepository
    {
        private readonly DataContext _context;

        public JustificationRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<JustificationEntity> Add(JustificationEntity justificationEntity)
        {
            if (justificationEntity is null)
                throw new ArgumentNullException(nameof(justificationEntity), "User cannot be null");

            var result = await _context.JustificationEntity.AddAsync(justificationEntity);
            await _context.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<List<JustificationEntity>> Get()
        {
            return await _context.JustificationEntity
                .AsNoTracking()
                .OrderBy(attendanceJus => attendanceJus.Id)
                .Select(attendanceJus => new JustificationEntity
                {
                    Id = attendanceJus.Id,
                    CreatedAt = attendanceJus.CreatedAt,                    
                    Reason = attendanceJus.Reason,
                    PointsEntity = attendanceJus.PointsEntity,
                })
                .ToListAsync();
        }

        public async Task<JustificationEntity?> GetById(int? id)
        {
            return await _context.JustificationEntity.FirstOrDefaultAsync(justificationEntity => justificationEntity.Id == id);
        }

        public JustificationEntity Update(JustificationEntity justificationEntity)
        {
            var response = _context.JustificationEntity.Update(justificationEntity);
            return response.Entity;
        }
    }
}
