using HotelBooking.Web.Helpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace HotelBooking.Web.TagHelpers
{
    [HtmlTargetElement("authorize-button", Attributes = "module, action")]
    public class AuthorizeButtonTagHelper : TagHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthorizeButtonTagHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        [HtmlAttributeName("module")]
        public string Module { get; set; }

        [HtmlAttributeName("action")]
        public string Action { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div"; 

            var user = _httpContextAccessor.HttpContext?.User;

            bool isAuthorized = Action.ToLower() switch
            {
                "create" => ButtonAuthorizationHelper.CanCreate(user, Module),
                "edit" => ButtonAuthorizationHelper.CanEdit(user, Module),
                "delete" => ButtonAuthorizationHelper.CanDelete(user, Module),
                "view" => ButtonAuthorizationHelper.CanView(user, Module),
                _ => false
            };

            if (!isAuthorized)
            {
                output.SuppressOutput();
            }
        }
    }
} 