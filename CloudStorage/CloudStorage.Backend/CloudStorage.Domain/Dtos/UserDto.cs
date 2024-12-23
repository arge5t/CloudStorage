﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudStorage.Domain.Dtos
{
    public class UserDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public long UsedSpace { get; set; }
        public long DiskSpace { get; set; }
        public string? Avatar { get; set; }
        public string RefreshToken {get; set; }
        public string AccessToken { get; set; }
    }
}
