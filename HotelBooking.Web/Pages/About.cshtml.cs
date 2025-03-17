using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HotelBooking.Web.Pages
{
    public class AboutModel : PageModel
    {
        public List<Service> Services { get; set; }
        public List<Testimonial> Testimonials { get; set; }
        public List<string> InstagramPhotos { get; set; }

        public void OnGet()
        {
            // Dữ liệu giả định cho dịch vụ
            Services = new List<Service>
            {
                new Service { Icon = "flaticon-reception-bell", Name = "Friendly Service" },
                new Service { Icon = "flaticon-serving-dish", Name = "Get Breakfast" },
                new Service { Icon = "flaticon-car", Name = "Transfer Services" },
                new Service { Icon = "flaticon-spa", Name = "Suits & SPA" },
                new Service { Icon = "ion-ios-bed", Name = "Cozy Rooms" }
            };

            // Dữ liệu giả định cho lời chứng thực
            Testimonials = new List<Testimonial>
            {
                new Testimonial { Name = "Gerald Hodson", Position = "Businessman", Image = "images/person_1.jpg", Review = "A small river named Duden flows by their place and supplies it with the necessary regelialia." },
                new Testimonial { Name = "Lisa Kim", Position = "Traveler", Image = "images/person_2.jpg", Review = "A paradise where roasted parts of sentences fly into your mouth." },
                new Testimonial { Name = "Michael Johnson", Position = "Entrepreneur", Image = "images/person_3.jpg", Review = "A place of serenity and comfort, perfect for relaxation." }
            };
            InstagramPhotos = new List<string>
            {
                "images/insta-1.jpg",
                "images/insta-2.jpg",
                "images/insta-3.jpg",
                "images/insta-4.jpg",
                "images/insta-5.jpg"
            };
        }
    }

    public class Service
    {
        public string Icon { get; set; }
        public string Name { get; set; }
    }

    public class Testimonial
    {
        public string Name { get; set; }
        public string Position { get; set; }
        public string Image { get; set; }
        public string Review { get; set; }
    }
}
