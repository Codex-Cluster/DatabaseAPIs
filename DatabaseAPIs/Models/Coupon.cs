using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DatabaseAPIs.Models
{
    public class Coupon
    {
        public string id { get; set; }
        public string code { get; set; }
        public string creator { get; set; }
        public float discount { get; set; }
        public string status { get; set; }
    }


}