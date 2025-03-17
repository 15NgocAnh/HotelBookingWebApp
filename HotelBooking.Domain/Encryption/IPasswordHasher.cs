namespace HotelBooking.Domain.Encryption
{
    public interface IPasswordHasher
    {
        string Hash(string password);
        bool verify(string password, string encryptedPassword);
        string md5(string password);
    }
}
