using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LmsApiApp.Application.Settings
{
    public class JwtSetting
    {
        public string SecretKey { get; set; } // JWT Secret Key
        public string Issuer { get; set; } // Issuer
        public string Audience { get; set; } // Audience
        public int ExpirationInMinutes { get; set; } // Token Expiration
  
    }
}
