using Domain.Dto;
using System;

namespace SessionBackend.Tools
{
    public class TokenGenerator
    {
        private int _expires;

        public TokenGenerator(int expires = 200)
        {
            _expires = expires;
        }

        public Token GenerateToken(params string[] strs)
        {
            var seed = Hash(string.Concat(strs));
            var value = GenerateString(seed);

            return new Token
            {
                Value = value,
                ExpiresOn = DateTime.Now.AddSeconds(_expires),
            };
        }

        public string Hash(string str)
        {
            return Math.Abs(str.GetHashCode()).ToString();
        }

        private static string GenerateString(string seed)
        {
            var value = seed + DateTime.Now.ToShortDateString()
                + DateTime.Now.ToShortTimeString()
                + DateTime.Now.Millisecond.ToString();

            return Math.Abs(value.GetHashCode()).ToString().PadLeft(10, '5');
        }
    }
}