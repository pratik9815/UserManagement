using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace UserManagement.Application.Services;
public class EncryptRouteService
{
    public static string EncryptRoute(string route, IConfiguration configuration)
    {
        byte[] iv = new byte[16];
        byte[] array;

        using (Aes aes = Aes.Create())
        {
            aes.Key = Encoding.UTF8.GetBytes(configuration["EncryptRoute:Key"]);
            aes.IV = iv;
            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                    {
                        streamWriter.Write(route);
                    }
                }
                array = memoryStream.ToArray();
            }
        }
        return Convert.ToBase64String(array);
    }
    public static string DecryptRoute(string cipherRoute, IConfiguration configuration)
    {
        byte[] iv = new byte[16];
        byte[] buffer = Convert.FromBase64String(cipherRoute);

        using(Aes aes = Aes.Create())
        {
            aes.IV = iv;
            aes.Key = Encoding.UTF8.GetBytes(configuration["EncryptRoute:Key"]);
            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            using (MemoryStream memoryStream = new MemoryStream(buffer))
            {
                using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                    {
                        return streamReader.ReadToEnd();
                    }
                }
            }
        }
    }
}
