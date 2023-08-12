using System.Text.Json.Serialization;

namespace DotNetArchitecture.Models
{
    public class CurrentUser
    {
        [JsonIgnore]
        public Guid UserId { get; set; }
    }
}
