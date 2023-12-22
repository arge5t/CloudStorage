using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudStorage.Domain.Entities
{
    public class Jwt
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Key { get; set; }

        public int MinutesToExpiration { get; set; }

        public TimeSpan ExpirationTime => TimeSpan.FromDays(MinutesToExpiration);
    }
}
