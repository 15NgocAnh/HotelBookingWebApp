namespace HotelBooking.Data.Models
{
    public abstract class BaseModel
    {
        public int changed_by { get; set; }
        public int created_by { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
        public bool is_deleted { get; set; } = false;
    }
}
