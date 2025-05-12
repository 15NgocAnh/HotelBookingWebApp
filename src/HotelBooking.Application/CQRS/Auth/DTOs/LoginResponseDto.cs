namespace HotelBooking.Application.CQRS.Auth.DTOs
{
    public class LoginResponseDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Token { get; set; }
        public List<string> Roles { get; set; }
    }
} 