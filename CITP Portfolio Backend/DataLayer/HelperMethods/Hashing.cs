using DataLayer.DomainObjects;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.HelperMethods
{
    public class Hashing 
    {
        protected const int saltBitsize = 64;
        protected const byte saltBytesize = saltBitsize / 8;
        protected const int hashBitsize = 256;
        protected const int hashBytesize = hashBitsize / 8;
    
        private HashAlgorithm sha256 = SHA256.Create();
        protected RandomNumberGenerator random = RandomNumberGenerator.Create();
    

        public (string hash, string salt) Hash(string password)
        {
            byte[] salt = new byte[saltBytesize];
            random.GetBytes(salt);

            string saltString = Convert.ToHexString(salt);
            string hash = HashSHA256(password, saltString);

            return (hash, saltString);
        }

        public bool Verify(string loginPassword, string hashedRegisteredPassword, string saltString)
        {
            string hashedLoginPassword = HashSHA256(loginPassword, saltString);

            if (hashedLoginPassword == hashedRegisteredPassword) return true;

            return false;
        }

        private string HashSHA256(string password, string saltString)
        {
            byte[] hashInput = Encoding.UTF8.GetBytes(saltString + password);
            byte[] hashOutput = sha256.ComputeHash(hashInput);
            return Convert.ToHexString(hashOutput);
        }

    }
}
