namespace HotelBooking.Application.CQRS.User.Commands.UpdateUser
{
    public record UpdateUserCommand : IRequest<Result>
    {
        public int Id { get; init; }
        public string Email { get; init; }
        public string FirstName { get; init; }
        public string LastName { get; init; }
        public string Phone { get; init; }
        public int RoleId { get; init; }
        public List<int> HotelIds { get; init; } = new List<int>();
    }
} 