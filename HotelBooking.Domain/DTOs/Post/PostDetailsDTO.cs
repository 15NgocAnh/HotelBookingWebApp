namespace HotelBooking.Domain.DTOs.Post
{
    public class PostDetailsDTO
    {
        public int id { get; set; }
        public string title { get; set; }
        public string caption { get; set; }
        public string author { get; set; }
        public string avatar_author { get; set; }
        public string file_url { get; set; }
        public DateTime? created_at { get; set; }
        public int likes { get; set; }
    }
}
