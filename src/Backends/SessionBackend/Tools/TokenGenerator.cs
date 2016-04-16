using Domain.Dto;
using System;
using Tools;

namespace SessionBackend.Tools
{
    public class TokenGenerator
    {
        private int _expires;
        private HashGenerator _hashGenerator;

        public TokenGenerator(int expires = 2000)
        {
            _expires = expires;
            _hashGenerator = new HashGenerator();
        }

        public Token GenerateToken(params string[] strs)
        {
            var seed = _hashGenerator.Generate(string.Concat(strs));
            var value = GenerateString(seed);

            return new Token
            {
                Value = value,
                ExpiresOn = DateTime.Now.AddSeconds(_expires),
            };
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