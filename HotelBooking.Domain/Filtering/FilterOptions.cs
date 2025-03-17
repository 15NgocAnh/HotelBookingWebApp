using HotelBooking.Domain.Constant;
using System.ComponentModel;

namespace HotelBooking.Domain.Filtering
{
    public class FilterOptions
    {
        public string? search_term { get; set; } = string.Empty;
        [DefaultValue(CJConstant.CURRENT_PAGE)]
        public int page { get; set; }
        [DefaultValue(CJConstant.PAGE_LIMIT)]
        public int limit { get; set; }
        public string? order_by { get; set; } = CJConstant.ORDER_BY;
    }
}
