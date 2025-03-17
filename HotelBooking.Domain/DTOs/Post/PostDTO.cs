
using System.Text.Json.Serialization;

namespace HotelBooking.Domain.DTOs.Post
{
    public class PostDTO
    {
        public string title { get; set; }
        public string caption { get; set; }
        public string? author { get; set; }
        public string file_type { get; set; }
        public string file_name { get; set; }

        [JsonIgnore]
        public string? file_url { get; set;} 
    }
}
