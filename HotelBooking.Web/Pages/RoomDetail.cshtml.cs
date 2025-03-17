using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HotelBooking.Web.Pages
{
    public class RoomDetailModel : PageModel
    {
        public RoomDetails Room { get; set; }
        public List<Rating> Ratings { get; set; }
        public List<BlogPost> RecentBlogs { get; set; }
        public List<Category> Categories { get; set; }

        public void OnGet()
        {
            Categories = new List<Category>
            {
                new Category { Name = "Properties", Count = 12 },
                new Category { Name = "Home", Count = 22 },
                new Category { Name = "House", Count = 37 },
                new Category { Name = "Villa", Count = 42 },
                new Category { Name = "Apartment", Count = 14 },
                new Category { Name = "Condominium", Count = 140 }
            };
            Room = new RoomDetails
            {
                Name = "Luxury Room",
                AvailableRooms = 4,
                Description = "When she reached the first hills of the Italic Mountains, she had a last view back on the skyline...",
                MaxPersons = 3,
                Size = 45,
                View = "Sea View",
                Bed = "1",
                MainImage = "/images/room-4.jpg",
                VideoUrl = "https://vimeo.com/45830194",
                Images = new List<string> { "/images/room-4.jpg", "/images/room-5.jpg", "/images/room-6.jpg" }
            };

            Ratings = new List<Rating>
            {
                new Rating { Star = 5, Count = 100 },
                new Rating { Star = 4, Count = 30 },
                new Rating { Star = 3, Count = 5 },
                new Rating { Star = 2, Count = 0 },
                new Rating { Star = 1, Count = 0 }
            };

            RecentBlogs = new List<BlogPost>
            {
                new BlogPost { Title = "Even the all-powerful Pointing has no control", ImageUrl = "/images/image_1.jpg", Date = DateTime.Now, Author = "Admin", Comments = 19 },
                new BlogPost { Title = "Even the all-powerful Pointing has no control", ImageUrl = "/images/image_2.jpg", Date = DateTime.Now, Author = "Admin", Comments = 19 },
                new BlogPost { Title = "Even the all-powerful Pointing has no control", ImageUrl = "/images/image_3.jpg", Date = DateTime.Now, Author = "Admin", Comments = 19 }
            };
        }
        public class Category
        {
            public string Name { get; set; } = string.Empty;
            public int Count { get; set; } // S? l??ng bài vi?t ho?c danh m?c
        }

        public class RoomDetails
        {
            public string Name { get; set; } = string.Empty;
            public int AvailableRooms { get; set; }
            public string Description { get; set; } = string.Empty;
            public int MaxPersons { get; set; }
            public int Size { get; set; }
            public string View { get; set; } = string.Empty;
            public string Bed { get; set; } = string.Empty;
            public string MainImage { get; set; } = string.Empty;
            public string VideoUrl { get; set; } = string.Empty;
            public List<string> Images { get; set; } = new List<string>();
        }

        public class Rating
        {
            public int Star { get; set; }  // S? sao (1-5)
            public int Count { get; set; } // S? l??ng ?ánh giá
        }

        public class BlogPost
        {
            public string Title { get; set; } = string.Empty;
            public string ImageUrl { get; set; } = string.Empty;
            public DateTime Date { get; set; }
            public string Author { get; set; } = string.Empty;
            public int Comments { get; set; }
        }
    }
}
