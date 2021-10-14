using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DatabaseAPIs.Models
{
    public class User
    {
        public string UserID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int Mobile { get; set; }
        public string Password { get; set; }
        public bool Active { get; set; }
        public List<string> Roles { get; set; }
        public List<string> Cart { get; set; }
        public List<string> Wishlist { get; set; }
    }
}