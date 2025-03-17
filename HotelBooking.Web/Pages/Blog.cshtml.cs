using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HotelBooking.Web.Pages
{
    public class BlogModel : PageModel
    {
        public List<BlogPost> BlogPosts { get; set; }

        public void OnGet()
        {
            BlogPosts = new List<BlogPost>
        {
            new BlogPost { Title = "Even the all-powerful Pointing has no control about the blind texts",
                          ImageUrl = "images/image_1.jpg",
                          Date = "Oct. 30, 2019",
                          Author = "Admin",
                          Comments = 3,
                          Content = "A small river named Duden flows by their place and supplies it with the necessary regelialia." },

            new BlogPost { Title = "Lorem Ipsum Dolor Sit Amet",
                          ImageUrl = "images/image_2.jpg",
                          Date = "Nov. 15, 2019",
                          Author = "John Doe",
                          Comments = 5,
                          Content = "Separated they live in Bookmarksgrove right at the coast of the Semantics." },

            new BlogPost { Title = "The quick brown fox jumps over the lazy dog",
                          ImageUrl = "images/image_3.jpg",
                          Date = "Dec. 5, 2019",
                          Author = "Jane Doe",
                          Comments = 2,
                          Content = "It is a paradisematic country, in which roasted parts of sentences fly into your mouth." }
        };
        }
    }

    public class BlogPost
    {
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public string Date { get; set; }
        public string Author { get; set; }
        public int Comments { get; set; }
        public string Content { get; set; }
        public List<string> Tags { get; set; }
    }
}
