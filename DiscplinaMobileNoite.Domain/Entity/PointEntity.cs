using DiscplinaMobileNoite.Domain.Enum;
using System.Text.Json.Serialization;

namespace DiscplinaMobileNoite.Domain.Entity
{
    public class PointEntity
    {
        [JsonIgnore]
        public int Id { get; set; }
        public int UserId { get; set; }
        //[JsonIgnore]
        public DateTime Date { get; set; }
        public TimeSpan? MorningEntry { get; set; }
        public TimeSpan? MorningExit { get; set; }
        public TimeSpan? AfternoonEntry { get; set; }
        public TimeSpan? AfternoonExit { get; set; }
        public PointStatus Status { get; set; }

        [JsonIgnore]
        public DateTime CreatedAt { get; set; }

        [JsonIgnore]
        public UserEntity? UserEntity { get; set; }

        [JsonIgnore]
        public ICollection<JustificationEntity> JustificationEntity { get; set; } = new List<JustificationEntity>();
    }
}