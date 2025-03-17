namespace HotelBooking.Domain.DTOs.Post
{
    public class PostValidatorDTO
    {
        public int id { get; set; }
        public string title { get; set; }
        public string caption { get; set; }
        public string author { get; set; }
        public string file_url { get; set; }
        public DateTime? created_at { get; set; }
        public int? changed_by { get; set; }
        public bool is_deleted { get; set; }
        public bool is_actived { get; set; }
    }
}
