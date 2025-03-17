using HotelBooking.Domain.Constant;
using System.ComponentModel;

namespace HotelBooking.Domain.Filtering
{
    public class FilterJobs
    {
        public string search_term { get; set; } = string.Empty;
        public string location { get; set; } = string.Empty;
        [DefaultValue(CJConstant.CURRENT_PAGE)]
        public int page { get; set; }
        [DefaultValue(CJConstant.PAGE_LIMIT)]
        public int limit { get; set; }
    }
}
