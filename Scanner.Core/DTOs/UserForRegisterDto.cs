﻿using Scanner.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scanner.Core.DTOs
{
    public class UserForRegisterDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string DeviceID { get; set; }
        public UserType UserType { get; set; }
    }
}
