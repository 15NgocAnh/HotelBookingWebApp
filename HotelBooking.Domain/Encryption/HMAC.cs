using Microsoft.Extensions.Configuration;

namespace HotelBooking.Domain.Encryption
{
    public class HMAC<T> : IEncryption<T>
    {
        private readonly IConfiguration _configuration;
        public HMAC(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        T IEncryption<T>.Decrypt(string data)
        {
            throw new NotImplementedException();
        }

        string IEncryption<T>.Encrypt(T data)
        {
            throw new NotImplementedException();
        }
    }
}
