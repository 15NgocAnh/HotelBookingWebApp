using HotelBooking.Domain.Common;

namespace HotelBooking.Domain.AggregateModels.HotelAggregate;
public class Hotel(string name,
                   string description,
                   string address,
                   string phone,
                   string email,
                   string website) : BaseEntity, IAggregateRoot
{
    public string Name { get; private set; } = name;
    public string Description { get; private set; } = description;
    public string Address { get; private set; } = address;
    public string Phone { get; private set; } = phone;
    public string Email { get; private set; } = email;
    public string Website { get; private set; } = website;

    public void Update(
        string name, 
        string description,
        string address,
        string phone,
        string email,
        string website)
    {
        Name = name;
        Description = description;
        Address = address;
        Phone = phone;
        Email = email;
        Website = website;
    }
}
