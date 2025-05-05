using System.Text.Json.Serialization;

namespace DiscplinaMobileNoite.Domain.Dto
{
    public class RecoverPasswordResponse
    {
        [JsonPropertyName("email")]
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}