using System.Security.Claims;

namespace HotelBooking.Web.Helpers
{
    public static class MenuAuthorizationHelper
    {
        public static bool CanAccessHotelManagement(ClaimsPrincipal user)
        {
            return user.IsInRole("SuperAdmin") || user.IsInRole("HotelManager");
        }

        public static bool CanAccessBuildingManagement(ClaimsPrincipal user)
        {
            return user.IsInRole("SuperAdmin") || user.IsInRole("HotelManager");
        }

        public static bool CanAccessRoomManagement(ClaimsPrincipal user)
        {
            return user.IsInRole("SuperAdmin") || user.IsInRole("HotelManager");
        }

        public static bool CanAccessExtraServiceManagement(ClaimsPrincipal user)
        {
            return user.IsInRole("SuperAdmin") || user.IsInRole("HotelManager");
        }

        public static bool CanAccessAmenityManagement(ClaimsPrincipal user)
        {
            return user.IsInRole("SuperAdmin") || user.IsInRole("HotelManager");
        }

        public static bool CanAccessBedManagement(ClaimsPrincipal user)
        {
            return user.IsInRole("SuperAdmin") || user.IsInRole("HotelManager");
        }

        public static bool CanAccessBookingManagement(ClaimsPrincipal user)
        {
            return user.IsInRole("SuperAdmin") || user.IsInRole("HotelManager") || user.IsInRole("FrontDesk");
        }

        public static bool CanAccessInvoiceManagement(ClaimsPrincipal user)
        {
            return user.IsInRole("SuperAdmin") || user.IsInRole("HotelManager") || user.IsInRole("FrontDesk");
        }

        public static bool CanAccessUserManagement(ClaimsPrincipal user)
        {
            return user.IsInRole("SuperAdmin") || user.IsInRole("HotelManager");
        }

        public static bool CanAccessStatistics(ClaimsPrincipal user)
        {
            return user.IsInRole("SuperAdmin") || user.IsInRole("HotelManager");
        }
    }
} 