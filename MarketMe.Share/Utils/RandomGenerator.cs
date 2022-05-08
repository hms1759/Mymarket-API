using System;
using System.Security.Cryptography;
using System.Text;

namespace MarketMe.Share.Utils
{
    public class RandomGenerator
    {
        public static string GenerateAPPKey()
        {
            using (var cryptoProvider = new RSACryptoServiceProvider())
            {
                byte[] secretKeyByteArray = new byte[32]; //256 bit
                                                          //  cryptoProvider.DecryptValue(secretKeyByteArray);
                cryptoProvider.Decrypt(secretKeyByteArray, false);
                return Convert.ToBase64String(secretKeyByteArray);
            }
        }


        public static string GenerateAPPKeys(string Username)
        {
            using (var rsa = new RSACryptoServiceProvider())
            {
                byte[] secretKeyByteArray = Encoding.Default.GetBytes(Username);
                byte[] encryptdata = rsa.Encrypt(secretKeyByteArray, false);
                string encryptstring = Convert.ToBase64String(encryptdata);
                return encryptstring.Substring(0, 6);
            }
        }

    }
}
