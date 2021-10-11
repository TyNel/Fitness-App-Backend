using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitness.Models.Domain
{
    public class AuthenticationConfiguration
    {
        public string TokenSecret { get; set; }

        public int TokenExpirationDays { get; set; }

        public string Issuer { get; set; }

        public string Audience { get; set;  }

        public string RefreshTokenSecret { get; set; }

        public int  RefreshTokenExpirationDays { get; set; }
    }
}
