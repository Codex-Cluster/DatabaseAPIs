using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data.SqlClient;
using System.Configuration;

using DatabaseAPIs.Models;

namespace DatabaseAPIs.Services
{
    public class DataLoadService
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["CodexDB"].ConnectionString;

        protected List<Book> LoadBooks()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandText = "SELECT * FROM Books";
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                return ConvertToBookList(dr);
            }
        }
        protected List<Category> LoadCategories()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandText = "SELECT * FROM Category";
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                return ConvertToCategoryList(dr);
            }
        }
        protected List<Book> ConvertToBookList(SqlDataReader reader)
        {
            var rows = new List<Book>();
            while (reader.Read())
            {
                // rows.Add(columns.ToDictionary(column => column, column => reader[column]));
                rows.Add(new Book()
                {
                    BookID = reader["BookID"].ToString(),
                    CatID = reader["CatID"].ToString(),
                    Image = reader["Image"].ToString(),
                    Author = reader["Author"].ToString(),
                    Title = reader["Title"].ToString(),
                    Format = reader["Format"].ToString(),
                    Rating = double.Parse(reader["Rating"].ToString()),
                    Price = double.Parse(reader["Price"].ToString()),
                    OldPrice = double.Parse(reader["OldPrice"].ToString()),
                    ISBN = reader["ISBN"].ToString(),
                    Description = reader["Description"].ToString(),
                    Year = reader["Year"].ToString(),
                    Position = Int32.Parse(reader["Position"].ToString()),
                    Status = reader["Status"].ToString()
                }) ;
            }
            return rows;
        }
        protected List<Category> ConvertToCategoryList(SqlDataReader reader)
        {
            var rows = new List<Category>();
            while (reader.Read())
            {
                // rows.Add(columns.ToDictionary(column => column, column => reader[column]));
                rows.Add(new Category()
                {
                    CatID = reader["CatID"].ToString(),
                    CatName = reader["CatName"].ToString(),
                    CatDescription = reader["CatDescription"].ToString(),
                    Position = Int32.Parse(reader["Position"].ToString()),
                    Status = reader["Status"].ToString(),
                    Image = reader["Image"].ToString(),
                    CreatedAt = reader["CreatedAt"].ToString()
                });
            }
            return rows;
        }
    }
}