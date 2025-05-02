using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBooking.Domain.Entities
{
    public class HourlyPrice
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int DynamicPricingRuleId { get; set; }
        public int HourSetting { get; set; }
        public decimal Price { get; set; }
        public virtual DynamicPricingRule DynamicPricingRule { get; set; }
    }
} 