using Scanner.Core.Abstract;
using Scanner.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scanner.Core.Models
{
    public class User : BaseEntity
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public bool IsActive { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpireDate { get; set; }
        public UserType UserType { get; set; }
        public string DeviceID { get; set; }
    }
}
