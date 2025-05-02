using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBooking.Domain.Entities
{
    public enum ExtraChargeType
    {
        EarlyCheckinByDay = 0,
        LateCheckoutByDay = 1,
        EarlyCheckinOvernight = 2,
        LateCheckoutOvernight = 3
    }

    public class ExtraCharge
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int DynamicPricingRuleId { get; set; }
        public ExtraChargeType ChargeType { get; set; }
        public int HourSetting { get; set; }
        public decimal Price { get; set; }
        public virtual DynamicPricingRule DynamicPricingRule { get; set; }
    }
} 