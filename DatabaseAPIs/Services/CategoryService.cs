using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data.SqlClient;
using System.Configuration;

using DatabaseAPIs.Models;
using DatabaseAPIs.Interfaces;

namespace DatabaseAPIs.Services
{
    public class CategoryService : DataLoadService, ICategory
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["CodexDB"].ConnectionString;

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

        public List<Category> GetCategories()
        {
            return Categories;

        }

        public List<Book> GetBooks(string CatID)
        {
            return Books.FindAll(x => x.CatID == CatID);
        }

        public Category AddCategory(Category category)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandText = String.Format(
                    "insert into Categories(CatID, CatName, CatDescription, Position, Status, Image, CreatedAt) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}');",
                    category.CatID, category.CatName, category.CatDescription, category.Position, category.Status, category.Image, category.CreatedAt
                    );
                con.Open();
                cmd.ExecuteNonQuery();
            }
            Categories = LoadCategories();
            return category;
        }

        public bool DeleteCategory(Category category)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandText = String.Format(
                    "DELETE FROM Categories WHERE CatID = '{0}'", category.CatID);
                con.Open();
                cmd.ExecuteNonQuery();
            }
            Categories = LoadCategories();
            return true;
        }
    }
}