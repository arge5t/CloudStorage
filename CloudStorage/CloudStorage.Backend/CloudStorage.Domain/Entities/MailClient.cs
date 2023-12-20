using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudStorage.Domain.Entities
{
    public class MailClient
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Secret { get; set; }
        public string Url { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
    }
}
