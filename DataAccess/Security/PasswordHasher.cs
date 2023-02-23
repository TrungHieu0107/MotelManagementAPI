using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace DataAccess.Security

{
    public class PasswordHasher
    {

        private const int SaltSize = 16;
        private const int HashSize = 20;
        private const int Iterations = 10000;

        public static string Hash(string password)
        {
            // Generate a salt value to use with the hash
            byte[] salt = new byte[SaltSize];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }

            // Hash the password with the salt
            var hashedBytes = new Rfc2898DeriveBytes(password, salt, Iterations);
            byte[] hash = hashedBytes.GetBytes(HashSize);

            // Combine the salt and hash into a single string
            byte[] hashBytes = new byte[SaltSize + HashSize];
            Array.Copy(salt, 0, hashBytes, 0, SaltSize);
            Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);
            string hashedPassword = Convert.ToBase64String(hashBytes);

            return hashedPassword;
        }

        public static bool Verify(string password, string hashedPassword)
        {
            // Convert the hashed password back to bytes
            byte[] hashBytes = Convert.FromBase64String(hashedPassword);

            // Extract the salt from the hash
            byte[] salt = new byte[SaltSize];
            Array.Copy(hashBytes, 0, salt, 0, SaltSize);

            // Hash the password with the extracted salt
            var hashedBytes = new Rfc2898DeriveBytes(password, salt, Iterations);
            byte[] hash = hashedBytes.GetBytes(HashSize);

            // Compare the computed hash with the stored hash
            for (int i = 0; i < HashSize; i++)
            {
                if (hashBytes[i + SaltSize] != hash[i])
                {
                    return false;
                }
            }

            return true;
        }


    }
}
