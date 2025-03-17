using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HotelBooking.Web.Pages
{
    public class BlogDetailModel : PageModel
    {
        public BlogPost Blog { get; set; }
        public List<Comment> Comments { get; set; }
        public List<Category> Categories { get; set; }
        public List<RecentBlog> RecentBlogs { get; set; }

        public void OnGet()
        {
            // D? li?u bài vi?t
            Blog = new BlogPost
            {
                Title = "Be A Creative Web Designer",
                ImageUrl = "images/image_1.jpg",
                Date = "Oct. 30, 2019",
                Author = "Lance Smith",
                Content = "Lorem ipsum dolor sit amet, consectetur adipisicing elit...",
                Tags = new List<string> { "Life", "Sport", "Tech", "Travel" }
            };

            // D? li?u danh sách bình lu?n
            Comments = new List<Comment>
            {
                new Comment { Name = "John Doe", Date = "Oct. 30, 2018 at 2:21pm", Avatar = "images/person_1.jpg", Message = "Great article, very helpful!" },
                new Comment { Name = "Jane Smith", Date = "Nov. 5, 2018 at 10:45am", Avatar = "images/person_2.jpg", Message = "Thanks for sharing this, very useful insights." }
            };

            // D? li?u danh m?c (Categories)
            Categories = new List<Category>
            {
                new Category { Name = "Properties", Count = 12 },
                new Category { Name = "Home", Count = 22 },
                new Category { Name = "House", Count = 37 },
                new Category { Name = "Villa", Count = 42 },
                new Category { Name = "Apartment", Count = 14 },
                new Category { Name = "Condominium", Count = 140 }
            };

            // D? li?u blog g?n ?ây
            RecentBlogs = new List<RecentBlog>
            {
                new RecentBlog { Title = "Even the all-powerful Pointing has no control about the blind texts", ImageUrl = "images/image_1.jpg", Date = "July, 04 2019", Author = "Admin", CommentsCount = 19 },
                new RecentBlog { Title = "Why Web Design Matters", ImageUrl = "images/image_2.jpg", Date = "June, 22 2019", Author = "Admin", CommentsCount = 10 },
                new RecentBlog { Title = "Best Practices for UI/UX", ImageUrl = "images/image_3.jpg", Date = "May, 15 2019", Author = "Admin", CommentsCount = 8 }
            };  
        }
    }

    public class Category
    {
        public string Name { get; set; }
        public int Count { get; set; }
    }

    public class RecentBlog
    {
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public string Date { get; set; }
        public string Author { get; set; }
        public int CommentsCount { get; set; }
    }

    public class Comment
    {
        public string Name { get; set; }
        public string Date { get; set; }
        public string Avatar { get; set; }
        public string Message { get; set; }
    }
}
