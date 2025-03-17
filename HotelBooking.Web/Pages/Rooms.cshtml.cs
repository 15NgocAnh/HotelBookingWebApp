using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HotelBooking.Web.Pages
{
    public class RoomsModel : PageModel
    {
        public List<Room> Rooms { get; set; } = new();

        public void OnGet()
        {
            Rooms = new List<Room>
            {
                new Room { Name = "King Room", ImageUrl = "images/room-6.jpg", Price = 120, Rating = 5, IsLeftAligned = true },
                new Room { Name = "Suite Room", ImageUrl = "images/room-1.jpg", Price = 120, Rating = 5, IsLeftAligned = true },
                new Room { Name = "Family Room", ImageUrl = "images/room-2.jpg", Price = 120, Rating = 5, IsLeftAligned = false },
                new Room { Name = "Deluxe Room", ImageUrl = "images/room-3.jpg", Price = 120, Rating = 5, IsLeftAligned = false },
                new Room { Name = "Luxury Room", ImageUrl = "images/room-4.jpg", Price = 120, Rating = 5, IsLeftAligned = true },
                new Room { Name = "Superior Room", ImageUrl = "images/room-5.jpg", Price = 120, Rating = 5, IsLeftAligned = true }
            };
        }

        public class Room
        {
            public string Name { get; set; }
            public string ImageUrl { get; set; }
            public int Price { get; set; }
            public int Rating { get; set; } = 5;
            public bool IsLeftAligned { get; set; }
        }
    }
}
