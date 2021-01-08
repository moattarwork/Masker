using System;
using System.Security.Cryptography;
using System.Text;

namespace Masker.Core
{
    public class StringFieldMasker : IFieldMasker<string>
    {
        private readonly HashAlgorithm _algorithm = new HMACMD5();

        public string Mask(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return value;

            var buffer = Encoding.UTF8.GetBytes(value);
            var hash = _algorithm.ComputeHash(buffer);
            
            return Convert.ToBase64String(hash).Substring(0, 20);
        }
    }
}