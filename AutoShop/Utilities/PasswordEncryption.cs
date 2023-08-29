using AutoShop.Helpers;
using System.Security.Cryptography;
using System.Text;

namespace AutoShop.Utilities
{
    public class PasswordEncryption
    {
        public static string EncryptPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password + ConfigurationHelper.config.GetSection("EncryptionKey").Value);
                byte[] hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
    }
}
