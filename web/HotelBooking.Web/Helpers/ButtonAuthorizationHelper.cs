using System.Security.Claims;

namespace HotelBooking.Web.Helpers
{
    public static class ButtonAuthorizationHelper
    {
        public static bool CanCreate(ClaimsPrincipal user, string module)
        {
            return module switch
            {
                "Hotel" => user.IsInRole("SuperAdmin"),
                "Building" => user.IsInRole("SuperAdmin") || user.IsInRole("HotelManager"),
                "RoomType" => user.IsInRole("SuperAdmin") || user.IsInRole("HotelManager"),
                "Room" => user.IsInRole("SuperAdmin") || user.IsInRole("HotelManager"),
                "ExtraCategory" => user.IsInRole("SuperAdmin") || user.IsInRole("HotelManager"),
                "ExtraItem" => user.IsInRole("SuperAdmin") || user.IsInRole("HotelManager"),
                "Amenity" => user.IsInRole("SuperAdmin") || user.IsInRole("HotelManager"),
                "BedType" => user.IsInRole("SuperAdmin") || user.IsInRole("HotelManager"),
                "Booking" => user.IsInRole("SuperAdmin") || user.IsInRole("HotelManager") || user.IsInRole("FrontDesk"),
                "Invoice" => user.IsInRole("SuperAdmin") || user.IsInRole("HotelManager") || user.IsInRole("FrontDesk"),
                "User" => user.IsInRole("SuperAdmin") || user.IsInRole("HotelManager"),
                "Role" => user.IsInRole("SuperAdmin"),
                _ => false
            };
        }

        public static bool CanEdit(ClaimsPrincipal user, string module)
        {
            return module switch
            {
                "Hotel" => user.IsInRole("SuperAdmin"),
                "Building" => user.IsInRole("SuperAdmin") || user.IsInRole("HotelManager"),
                "Room" => user.IsInRole("SuperAdmin") || user.IsInRole("HotelManager"),
                "RoomType" => user.IsInRole("SuperAdmin") || user.IsInRole("HotelManager"),
                "ExtraCategory" => user.IsInRole("SuperAdmin") || user.IsInRole("HotelManager"),
                "ExtraItem" => user.IsInRole("SuperAdmin") || user.IsInRole("HotelManager"),
                "Amenity" => user.IsInRole("SuperAdmin") || user.IsInRole("HotelManager"),
                "BedType" => user.IsInRole("SuperAdmin") || user.IsInRole("HotelManager"),
                "Booking" => user.IsInRole("SuperAdmin") || user.IsInRole("HotelManager") || user.IsInRole("FrontDesk"),
                "Invoice" => user.IsInRole("SuperAdmin") || user.IsInRole("HotelManager") || user.IsInRole("FrontDesk"),
                "User" => user.IsInRole("SuperAdmin") || user.IsInRole("HotelManager"),
                "Role" => user.IsInRole("SuperAdmin"),
                _ => false
            };
        }

        public static bool CanDelete(ClaimsPrincipal user, string module)
        {
            return module switch
            {
                "Hotel" => user.IsInRole("SuperAdmin"),
                "Building" => user.IsInRole("SuperAdmin") || user.IsInRole("HotelManager"),
                "Room" => user.IsInRole("SuperAdmin") || user.IsInRole("HotelManager"),
                "RoomType" => user.IsInRole("SuperAdmin") || user.IsInRole("HotelManager"),
                "ExtraCategory" => user.IsInRole("SuperAdmin") || user.IsInRole("HotelManager"),
                "ExtraItem" => user.IsInRole("SuperAdmin") || user.IsInRole("HotelManager"),
                "Amenity" => user.IsInRole("SuperAdmin") || user.IsInRole("HotelManager"),
                "BedType" => user.IsInRole("SuperAdmin") || user.IsInRole("HotelManager"),
                "Booking" => user.IsInRole("SuperAdmin") || user.IsInRole("HotelManager"),
                "Invoice" => user.IsInRole("SuperAdmin") || user.IsInRole("HotelManager"),
                "User" => user.IsInRole("SuperAdmin") || user.IsInRole("HotelManager"),
                "Role" => user.IsInRole("SuperAdmin"),
                _ => false
            };
        }

        public static bool CanView(ClaimsPrincipal user, string module)
        {
            return module switch
            {
                "Hotel" => user.IsInRole("SuperAdmin"),
                "Building" => user.IsInRole("SuperAdmin") || user.IsInRole("HotelManager"),
                "Room" => user.IsInRole("SuperAdmin") || user.IsInRole("HotelManager"),
                "RoomType" => user.IsInRole("SuperAdmin") || user.IsInRole("HotelManager"),
                "ExtraCategory" => user.IsInRole("SuperAdmin") || user.IsInRole("HotelManager"),
                "ExtraItem" => user.IsInRole("SuperAdmin") || user.IsInRole("HotelManager"),
                "Amenity" => user.IsInRole("SuperAdmin") || user.IsInRole("HotelManager"),
                "BedType" => user.IsInRole("SuperAdmin") || user.IsInRole("HotelManager"),
                "Booking" => user.IsInRole("SuperAdmin") || user.IsInRole("HotelManager") || user.IsInRole("FrontDesk"),
                "Invoice" => user.IsInRole("SuperAdmin") || user.IsInRole("HotelManager") || user.IsInRole("FrontDesk"),
                "User" => user.IsInRole("SuperAdmin") || user.IsInRole("HotelManager"),
                "Role" => user.IsInRole("SuperAdmin"),
                _ => false
            };
        }
    }
} 