using DiscplinaMobileNoite.Domain.Enum;
using System.Text.Json.Serialization;

namespace DiscplinaMobileNoite.Domain.Entity
{
    public class JustificationEntity
    {
        public int Id { get; set; }
        public int PointId { get; set; }
        public string? Reason { get; set; }
        public JustificationStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        
        [JsonIgnore]
        public PointEntity? PointsEntity { get; set; }
    }
}