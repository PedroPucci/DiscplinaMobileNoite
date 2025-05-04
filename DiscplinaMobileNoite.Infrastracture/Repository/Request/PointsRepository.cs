using DiscplinaMobileNoite.Domain.Entity;
using DiscplinaMobileNoite.Infrastracture.Connections;
using DiscplinaMobileNoite.Infrastracture.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DiscplinaMobileNoite.Infrastracture.Repository.Request
{
    public class PointsRepository : IPointsRepository
    {
        private readonly DataContext _context;

        public PointsRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<PointEntity> Add(PointEntity pointEntity)
        {
            if (pointEntity is null)
                throw new ArgumentNullException(nameof(pointEntity), "Attendance Record cannot be null");

            var result = await _context.Points.AddAsync(pointEntity);
            await _context.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<List<PointEntity>> Get()
        {
            return await _context.Points
                .AsNoTracking()
                .OrderBy(points => points.Id)
                .Select(points => new PointEntity
                {
                    Id = points.Id,
                    UserId = points.Id,
                    AfternoonEntry = points.AfternoonEntry,
                    AfternoonExit = points.AfternoonExit,
                    CreatedAt = points.CreatedAt,
                    MorningEntry = points.MorningEntry,
                    MorningExit = points.MorningExit,
                    Status = points.Status,
                })
                .ToListAsync();
        }

        public async Task<PointEntity?> GetById(int? id)
        {
            return await _context.Points.FirstOrDefaultAsync(pointEntity => pointEntity.Id == id);
        }

        public PointEntity Update(PointEntity pointEntity)
        {
            var response = _context.Points.Update(pointEntity);
            return response.Entity;
        }

        public async Task<List<PointEntity>> GetAllByUserIdAndDate(int userId, DateTime date)
        {
            var utcDate = DateTime.SpecifyKind(date.Date, DateTimeKind.Utc);

            return await _context.Points
                .Where(p => p.UserId == userId && p.Date.Date == utcDate.Date)
                .ToListAsync();
        }
    }
}