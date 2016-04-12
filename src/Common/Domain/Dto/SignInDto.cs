using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dto
{
    public class SignInDto
    {
        public Token Token { get; set; }

        public WorkerDto Worker { get; set; }
    }
}
