using HotelBooking.Domain.AggregateModels.UserAggregate;

namespace HotelBooking.Domain.Interfaces.Repositories
{
    public interface IUserHotelRepository : IGenericRepository<UserHotel>
    {
        Task DeleteAllByUserIdAsync(int userId);
    }
}
