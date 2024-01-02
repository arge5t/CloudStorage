using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudStorage.Domain.Entities
{
    public class File
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public long Size { get; set; } = 0;
        public string Path { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? EditTime { get; set; }
        public Guid UserId { get; set; }
        public Guid ParentId { get; set; }
    }
}
