using System.Security.Cryptography;
using System.Text;

namespace ProgettoParadigmi.Api.Utils
{
    public class Encrypter
    {
        public static string ComputeHash(string input)
        {
            using SHA256 sha256 = SHA256.Create();
            var inputBytes = Encoding.UTF8.GetBytes(input);
            var hashBytes = sha256.ComputeHash(inputBytes);
            var builder = new StringBuilder();
            foreach (var hashByte in hashBytes)
            {
                builder.Append(hashByte.ToString("x2"));
            }
            return builder.ToString();
        }
    }
}
