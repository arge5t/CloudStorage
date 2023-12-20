﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudStorage.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool isActivated { get; set; }
        public string ActivationLink { get; set; }
        public int DiskSpace { get; set; }
        public int UsedSpace { get; set; }
        public string? Avatar { get; set; }
    }
}