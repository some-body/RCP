using System;

namespace Domain.Dto
{
    public class Token
    {
        public string Value { get; set; }

        public DateTime ExpiresOn { get; set; }
    }
}