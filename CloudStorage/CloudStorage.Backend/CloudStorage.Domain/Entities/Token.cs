﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudStorage.Domain.Entities
{
    public class Token
    {
        public Guid Id { get; set; }
        public Guid Refresh { get; set; }
        public string Access { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}
