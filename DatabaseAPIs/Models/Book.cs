using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DatabaseAPIs.Models
{
    public class Book
    {
        public string BookID { get; set; }
        public string CatID { get; set; }
        public string Image { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Format { get; set; }
        public double Rating { get; set; }
        public double Price { get; set; }
        public double OldPrice { get; set; }
        public string ISBN { get; set; }
        public string Description { get; set; }
        public string Year { get; set; }
        public int Position { get; set; }
        public string Status { get; set; }
    }
}