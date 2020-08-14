using Scanner.Core.Abstract;
using Scanner.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scanner.Core.Models
{
    public class Billing : BaseEntity
    {
        public BillingType BillingType { get; set; }
        public string No { get; set; }
        public DateTime Date { get; set; }
        public double TotalPrice { get; set; }
        public string CustomerName { get; set; }
        public string CustomerSurname { get; set; }
        public int SubscribeId { get; set; }

        public virtual Subscribe Subscribe { get; set; }
    }
}
