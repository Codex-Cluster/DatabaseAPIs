using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DatabaseAPIs.Models
{
    public class Order
    {
        public int orderID { get; set; }
        public string userID { get; set; }
        public string books { get; set; }
        public string datetime { get; set; }
        public string coupon { get; set; }
        public float amt { get; set; }
        public string address { get; set; }
    }
}