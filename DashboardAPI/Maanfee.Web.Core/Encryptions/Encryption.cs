using System.Security.Cryptography;
using System.Text;

namespace Maanfee.Web.Core.Encryptions
{
    // رمزنگاری دوطرفه (AES) پسورد کاربران برای ستون PLA؛ برخلاف PasswordHash خودِ Identity
    // که یک‌طرفه است، این مقدار فقط از طریق برنامه (با دسترسی مدیر) قابل بازگشت به حالت اصلی است.
    public static class Encryption
    {
        private const string SecretKey = "Maanfee-Password-Encryption-Secret-Key";
        private static byte[] Key => SHA256.HashData(Encoding.UTF8.GetBytes(SecretKey));

        public static string Encrypt(this string plainText)
        {
            using var aes = Aes.Create();
            aes.Key = Key;
            aes.GenerateIV();

            using var encryptor = aes.CreateEncryptor();
            var plainBytes = Encoding.UTF8.GetBytes(plainText);
            var cipherBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

            var result = new byte[aes.IV.Length + cipherBytes.Length];
            Buffer.BlockCopy(aes.IV, 0, result, 0, aes.IV.Length);
            Buffer.BlockCopy(cipherBytes, 0, result, aes.IV.Length, cipherBytes.Length);

            return Convert.ToBase64String(result);
        }

        public static string Decrypt(this string cipherText)
        {
            var fullCipher = Convert.FromBase64String(cipherText);

            using var aes = Aes.Create();
            aes.Key = Key;

            var ivLength = aes.BlockSize / 8;
            var iv = new byte[ivLength];
            var cipher = new byte[fullCipher.Length - ivLength];
            Buffer.BlockCopy(fullCipher, 0, iv, 0, ivLength);
            Buffer.BlockCopy(fullCipher, ivLength, cipher, 0, cipher.Length);

            aes.IV = iv;
            using var decryptor = aes.CreateDecryptor();
            var plainBytes = decryptor.TransformFinalBlock(cipher, 0, cipher.Length);

            return Encoding.UTF8.GetString(plainBytes);
        }

        // رکوردهای قدیمی هنوز به صورت متن ساده ذخیره شده‌اند؛ این متد سعی می‌کند مقدار را رمزگشایی کند
        // و اگر مقدار اصلا رمزنگاری‌شده نبود (متن ساده‌ی قدیمی بود)، همان مقدار را بدون تغییر برمی‌گرداند.
        public static string TryDecrypt(this string? value)
        {
            if (string.IsNullOrEmpty(value))
                return value ?? string.Empty;

            try
            {
                return value.Decrypt();
            }
            catch
            {
                return value;
            }
        }
    }
}
