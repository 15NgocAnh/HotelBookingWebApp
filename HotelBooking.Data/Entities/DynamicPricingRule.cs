using HotelBooking.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBooking.Domain.Entities
{
    public class DynamicPricingRule : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        public int RoomTypeId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int DatePrice { get; set; }

        public int NightPrice { get; set; }

        public int HourPrice { get; set; }

        public int? MonthPrice { get; set; }

        public int ExtraAdult { get; set; }

        public int ExtraChild { get; set; }

        public DynamicPricingRuleType Type { get; set; }

        // Navigation properties
        public virtual RoomType RoomType { get; set; }
        public virtual ICollection<HourlyPrice> HourlyPrices { get; set; }
        public virtual ICollection<ExtraCharge> ExtraCharges { get; set; }
    }
} 