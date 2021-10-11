using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitness.Models.Domain
{
    public class RefreshToken
    {
        public string Token { get; set; }

        public int UserId { get; set; }
    }
}
