using System.Globalization;

namespace HotelBooking.Web.Extensions
{
    public static class DecimalExtensions
    {
        public static string ToVnd(this decimal value)
        {
            return value.ToString("N0", new CultureInfo("vi-VN")) + " VNĐ";
        }
    }
} 