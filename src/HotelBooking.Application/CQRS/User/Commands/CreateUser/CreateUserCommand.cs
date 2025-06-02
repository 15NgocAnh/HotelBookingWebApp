namespace HotelBooking.Application.CQRS.User.Commands.CreateUser
{
    public record CreateUserCommand : IRequest<Result<int>>
    {
        public string Email { get; init; }
        public string Password { get; init; }
        public string FirstName { get; init; }
        public string LastName { get; init; }
        public string PhoneNumber { get; init; }
        public int RoleId { get; init; }
        public List<int> HotelIds { get; init; } = new List<int>();
    }
} 