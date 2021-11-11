using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DatabaseAPIs.Models
{
    public class Promoted
    {
        public int id { get; set; }
        public string bookID { get; set; }
        public string expiresOn { get; set; }
    }
}