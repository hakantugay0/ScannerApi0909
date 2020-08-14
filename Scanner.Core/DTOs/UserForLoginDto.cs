using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Scanner.Core.DTOs
{
    public class UserForLoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string DeviceID { get; set; }
    }
}
