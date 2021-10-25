using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DatabaseAPIs.Models
{
    public class Order : Book
    {
        public int orderID { get; set; }
        public string userID { get; set; }
        public Dictionary<string,int> books { get; set; }
        public string datetime { get; set; }
        public string coupon { get; set; }
        public float amt { get; set; }
        public string address { get; set; }
        public int qty { get; set; }
    }

    public class OrderDetails
    {
        public int n_orders { get; set; }
        public List<Dictionary<string, Order>> orderDetails { get; set; }
    }
}