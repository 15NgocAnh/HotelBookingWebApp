using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace HotelBooking.Web.Pages
{
    public class IndexModel : PageModel
    {
        public DateTime CheckInDate { get; set; } = DateTime.Today;
        public DateTime CheckOutDate { get; set; } = DateTime.Today.AddDays(1);
        public string RoomType { get; set; }
        public int Guests { get; set; }
        public List<Room> Rooms { get; set; } = [];

        public void OnGet() {
            Rooms = new List<Room>
            {
                new Room { Img = "room-6.jpg", Name = "King Room", Price = 120, Reverse = false },
                new Room { Img = "room-1.jpg", Name = "Suite Room", Price = 120, Reverse = false },
                new Room { Img = "room-2.jpg", Name = "Family Room", Price = 120, Reverse = true },
                new Room { Img = "room-3.jpg", Name = "Deluxe Room", Price = 120, Reverse = true },
                new Room { Img = "room-4.jpg", Name = "Luxury Room", Price = 120, Reverse = false },
                new Room { Img = "room-5.jpg", Name = "Superior Room", Price = 120, Reverse = false }
            };
        }
    }

    public class Room
    {
        public string Img { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public bool Reverse { get; set; }
    }
}
