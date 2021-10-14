using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Configuration;

using DatabaseAPIs.Models;
using DatabaseAPIs.Interfaces;

namespace DatabaseAPIs.Services
{
    public class CategoryService : DataLoadService, ICategory
    {
        public List<Category> Categories = new List<Category>();
        public List<Book> Books = new List<Book>();
        private static CategoryService Instance = null;
        public static CategoryService instantiateDB()
        {
            if (Instance == null)
            {
                Instance = new CategoryService();
            }
            return Instance;
        }
        private CategoryService()
        {
            Categories = LoadCategories();
            Books = LoadBooks();
        }

        public List<string> GetCategories()
        {
            return Categories.Select(x => x.CatName).ToList();

        }

        public List<Book> GetBooks(string CatID)
        {
            return Books.FindAll(x => x.CatID == CatID);
        }
    }
}