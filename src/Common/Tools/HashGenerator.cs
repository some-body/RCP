using System;

namespace Tools
{
    public class HashGenerator
    {
        public string Generate(string str)
        {
            return Math.Abs(str.GetHashCode()).ToString();
        }
    }
}
