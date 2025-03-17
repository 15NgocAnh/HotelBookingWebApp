using System.Security.Cryptography;
using System.Text;

namespace HotelBooking.Domain.Encryption
{
    public class Bcrypt : IPasswordHasher
    {
        public string Hash(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public string md5(string password)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = md5.ComputeHash(inputBytes);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2")); 
                }
                return sb.ToString();
            }
        }

        public bool verify(string password, string encryptedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, encryptedPassword); 

        }
    }
}
