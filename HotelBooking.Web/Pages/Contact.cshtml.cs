using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Web.Pages
{
    public class ContactModel : PageModel
    {
        [BindProperty]
        public ContactForm ContactForm { get; set; }

        public void OnGet()
        {
            // Dữ liệu mặc định cho trang
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Xử lý gửi email hoặc lưu vào database nếu cần
            TempData["SuccessMessage"] = "Your message has been sent successfully!";
            return RedirectToPage();
        }
    }

    public class ContactForm
    {
        [Required]
        [Display(Name = "Your Name")]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Your Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Subject")]
        public string Subject { get; set; }

        [Required]
        [Display(Name = "Message")]
        public string Message { get; set; }
    }
}
