using HotelBooking.Domain.DTOs.Room;
using HotelBooking.Web.Pages.Abstract;

namespace HotelBooking.Web.Pages
{
    public class RoomDetailModel : AbstractPageModel
    {
        public RoomDetailModel(IConfiguration configuration, IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor) : base(configuration, httpClientFactory, httpContextAccessor)
        {
        }

        public RoomDetailsDTO Room { get; set; }
        public List<Rating> Ratings { get; set; }
        public List<BlogPost> RecentBlogs { get; set; }
        public List<Category> Categories { get; set; }

        public async Task OnGetAsync(int id)
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
            Room = await GetAsync<RoomDetailsDTO>($"api/v1/room/{id}") ?? new();

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
