using System.Text.Json.Serialization;

namespace DiscplinaMobileNoite.Domain.Entity
{
    public class UserEntity
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public int Workload { get; set; }
        public string? PhoneNumber { get; set; }
        [JsonIgnore]
        public DateTime? CreatedAt { get; set; }

        [JsonIgnore]
        public ICollection<PointEntity> PointsEntity { get; set; } = new List<PointEntity>();
    }
}