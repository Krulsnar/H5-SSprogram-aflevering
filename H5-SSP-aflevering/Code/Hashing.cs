using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;

namespace H5ServersideProgrammering.Codes
{
    public class Hashing
    {
        public string GetHashedText_MD5(string valueToHash)
        {
            byte[] valueAsBytes = ASCIIEncoding.ASCII.GetBytes(valueToHash);
            byte[] valueHashed = MD5.HashData(valueAsBytes);
            string hashedValueAsString = Convert.ToBase64String(valueHashed);

            return hashedValueAsString;
        }

        public string GetHashedText_bcrypt(string valueToHash)
        {
            string hashedValueAsString = BCrypt.Net.BCrypt.HashPassword(valueToHash);
            
            return hashedValueAsString;
        }

        public string GetHashedText_PBKDF2(string valueToHash)
        {
            byte[] salt = ASCIIEncoding.ASCII.GetBytes("testtest1234");
            Rfc2898DeriveBytes hashed = new Rfc2898DeriveBytes(valueToHash, salt);
            string hashedValueAsString = Convert.ToBase64String(hashed.GetBytes(32));

            return hashedValueAsString;
        }
    }
}
