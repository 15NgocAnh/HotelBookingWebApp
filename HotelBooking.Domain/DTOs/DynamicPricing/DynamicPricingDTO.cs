using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Domain.DTOs.DynamicPricing
{
    public class DynamicPricingDTO
    {
        public int Id { get; set; }

        [Display(Name = "Tên mức giá")]
        public string Name { get; set; }

        [Display(Name = "Loại phòng")]
        public int RoomTypeId { get; set; }

        [Display(Name = "Tên loại phòng")]
        public string RoomTypeName { get; set; }

        [Display(Name = "Ngày bắt đầu")]
        public DateTime StartDate { get; set; }

        [Display(Name = "Ngày kết thúc")]
        public DateTime EndDate { get; set; }

        public int DatePrice { get; set; }

        public int NightPrice { get; set; }

        public int HourPrice { get; set; }

        public int? MonthPrice { get; set; }

        public int ExtraAdult { get; set; }

        public int ExtraChild { get; set; }

        public Entities.DynamicPricingRuleType Type { get; set; }

        public List<HourlyPriceDTO> HourlyPrices { get; set; }
        public List<ExtraChargeDTO> ExtraCharges { get; set; }
    }

    public class CreateDynamicPricingDTO
    {
        [Required(ErrorMessage = "Tên mức giá là bắt buộc")]
        [Display(Name = "Tên quy tắc")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Loại phòng là bắt buộc")]
        [Display(Name = "Loại phòng")]
        public int RoomTypeId { get; set; }

        [Required(ErrorMessage = "Ngày bắt đầu là bắt buộc")]
        [Display(Name = "Ngày bắt đầu")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "Ngày kết thúc là bắt buộc")]
        [Display(Name = "Ngày kết thúc")]
        public DateTime EndDate { get; set; }

        public int DatePrice { get; set; }

        public int NightPrice { get; set; }

        public int HourPrice { get; set; }

        public int? MonthPrice { get; set; }

        public int? ExtraAdult { get; set; }

        public int? ExtraChild { get; set; }

        public Entities.DynamicPricingRuleType Type { get; set; }

        public List<HourlyPriceDTO>? HourlyPrices { get; set; }
        public List<ExtraChargeDTO>? ExtraCharges { get; set; }
    }

    public class UpdateDynamicPricingDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên mức giá là bắt buộc")]
        [Display(Name = "Tên quy tắc")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Loại phòng là bắt buộc")]
        [Display(Name = "Loại phòng")]
        public int RoomTypeId { get; set; }

        [Required(ErrorMessage = "Ngày bắt đầu là bắt buộc")]
        [Display(Name = "Ngày bắt đầu")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "Ngày kết thúc là bắt buộc")]
        [Display(Name = "Ngày kết thúc")]
        public DateTime EndDate { get; set; }

        public int DatePrice { get; set; }

        public int NightPrice { get; set; }

        public int HourPrice { get; set; }

        public int? MonthPrice { get; set; }

        public int? ExtraAdult { get; set; }

        public int? ExtraChild { get; set; }

        public Entities.DynamicPricingRuleType Type { get; set; }

        public List<HourlyPriceDTO>? HourlyPrices { get; set; }
        public List<ExtraChargeDTO>? ExtraCharges { get; set; }
    }

    public class HourlyPriceDTO
    {
        public int HourSetting { get; set; }
        public decimal Price { get; set; }
    }

    public class ExtraChargeDTO
    {
        public Entities.ExtraChargeType ChargeType { get; set; }
        public int HourSetting { get; set; }
        public decimal Price { get; set; }
    }
} 