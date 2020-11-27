using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace PruebaArandaApi.Helpers
{
    /// <summary>
    /// Utilidad para el manejo de las contraseñas
    /// </summary>
    public class LoginUtils
    {
        /// <summary>
        /// Encriptar el password
        /// </summary>
        /// <param name="password">Password</param>
        /// <returns>Password encriptado</returns>
        public string Encrypt(string password)
        {
            byte[] clearBytes = Encoding.Unicode.GetBytes(password.Trim());
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(password.Trim(), new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    password = Convert.ToBase64String(ms.ToArray());
                }
            }
            return password;
        }

        /// <summary>
        /// Generar codigo de X cantidad de digitos
        /// </summary>
        /// <returns>Cadena alfanumerica</returns>
        public string GetRandomString()
        {
            const string charPool = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890";
            var length = 8;
            var rs = new StringBuilder();
            var randomNumber = new Random();

            while (length > 0)
            {
                rs.Append(charPool[(int)(randomNumber.NextDouble() * charPool.Length)]);
                length--;
            }
            return rs.ToString();
        }
    }
}
