using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HotelBooking.Web.Pages
{
    public class RestaurantModel : PageModel
    {
        public List<MenuItem> MenuItems { get; set; }

        public void OnGet()
        {
            // Dữ liệu giả định cho menu
            MenuItems = new List<MenuItem>
        {
            new MenuItem { Name = "Grilled Crab with Onion", Price = 20.00, Image = "images/menu-1.jpg" },
            new MenuItem { Name = "Grilled Crab with Onion", Price = 20.00, Image = "images/menu-2.jpg" },
            new MenuItem { Name = "Grilled Crab with Onion", Price = 20.00, Image = "images/menu-3.jpg" },
            new MenuItem { Name = "Grilled Crab with Onion", Price = 20.00, Image = "images/menu-4.jpg" },
            new MenuItem { Name = "Grilled Crab with Onion", Price = 20.00, Image = "images/menu-5.jpg" },
            new MenuItem { Name = "Grilled Crab with Onion", Price = 20.00, Image = "images/menu-6.jpg" },
            new MenuItem { Name = "Grilled Crab with Onion", Price = 20.00, Image = "images/menu-7.jpg" },
            new MenuItem { Name = "Grilled Crab with Onion", Price = 20.00, Image = "images/menu-8.jpg" },
            new MenuItem { Name = "Grilled Crab with Onion", Price = 20.00, Image = "images/menu-9.jpg" }
        };
        }
    }

    public class MenuItem
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public string Image { get; set; }
    }
}
