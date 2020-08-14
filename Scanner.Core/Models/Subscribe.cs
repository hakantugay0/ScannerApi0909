using Scanner.Core.Abstract;
using Scanner.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scanner.Core.Models
{
    public class Subscribe : BaseEntity
    {
        public int UserId { get; set; }
        public UserType UserType { get; set; }
        public int Remaining { get; set; }
        public string GoogleToken { get; set; }
        public DateTime? GoogleTokenExpireDate { get; set; }
        public int BillingId { get; set; }

        public virtual Billing Billing { get; set; }
    }
}
