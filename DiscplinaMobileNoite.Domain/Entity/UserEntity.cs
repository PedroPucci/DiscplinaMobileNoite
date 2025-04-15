using System.Text.Json.Serialization;

namespace DiscplinaMobileNoite.Domain.Entity
{
    public class UserEntity
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Workload { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

        [JsonIgnore]
        public ICollection<PointEntity> PointsEntity { get; set; } = new List<PointEntity>();
    }
}