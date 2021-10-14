using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DatabaseAPIs.Models
{
    public class Category
    {
        public string CatID { get; set; }
        public string CatName { get; set; }
        public string CatDescription { get; set; }
        public int Position { get; set; }
        public string Status { get; set; }
        public string Image { get; set; }
        public string CreatedAt { get; set; }
    }
}