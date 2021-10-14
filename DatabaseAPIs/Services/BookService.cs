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
    public class BookService : DataLoadService, IBook
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["CodexDB"].ConnectionString;
        public List<Book> Books = new List<Book>();
        private static BookService Instance = null;
        public static BookService instantiateDB()
        {
            if (Instance == null)
            {
                Instance = new BookService();
            }
            return Instance;
        }
        private BookService()
        {
            Books = LoadBooks();
        }


        public List<Book> GetData()
        {
            return Books;
        }

        public Book GetData(string isbn)
        {
            return Books.FirstOrDefault(book => book.ISBN == isbn);
        }

        public string PostData(Book book)
        {
            int _pos = 0;
            string _bookID;
            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandText = String.Format("select count(*) from Books");
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    _pos = Int32.Parse(dr[0].ToString());
                }
                _bookID = "book" + new string('0', (5 - _pos.ToString().Length)) + _pos.ToString();
                cmd.CommandText = String.Format(
                    "insert into Books (Author, Title, CatID, ISBN, Image, Rating, Format, Price, OldPrice, Year, Position, Status, Description, BookID) values ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}', '{13}', )",
                    book.Author, book.Title, book.CatID, book.ISBN, book.Image, book.Rating, book.Format, book.Price, book.OldPrice, book.Year, _pos, book.Status, book.Description, _bookID
                    );
                
                cmd.ExecuteNonQuery();
            }
            Books.Add(book);
            return String.Format(
                "Successfully added book! Author: {0} Title: {1} CatID: {2} ISBN: {3} Year: {4}",
                book.Author, book.Title, book.CatID, book.ISBN, book.Year, book.Rating, book.Format, book.Price, book.OldPrice
                );
        }

        public string PutData(Book book)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandText = String.Format(
                    "update Books set Author='{0}', Title='{1}', CatID='{2}', ISBN='{3}', Image='{4}', Rating='{5}', Format='{6}', Price='{7}', OldPrice='{8}', Description='{9}', Position='{10}' ,Status='{11}' Year='{12}' where ISBN='{3}'",
                    book.Author, book.Title, book.CatID, book.ISBN, book.Image, book.Rating, book.Format, book.Price, book.OldPrice, book.Description, book.Position, book.Status, book.Year
                    );
                con.Open();
                cmd.ExecuteNonQuery();
            }
            Books = LoadBooks();
            return String.Format(
                "Successfully updated book! Author: {0} Title: {1} CatID: {2} ISBN: {3} Price: {8}",
                book.Author, book.Title, book.CatID, book.ISBN, book.Price, book.OldPrice, book.Rating, book.Format, book.Image
                );
        }

        public string DeleteData(string isbn)
        {

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandText = String.Format("DELETE FROM Books WHERE ISBN='{0}'", isbn);
                con.Open();
                cmd.ExecuteNonQuery();
            }
            Books.Remove(Books.FirstOrDefault(book => book.ISBN == isbn));
            return String.Format("Deleted book where ISBN == {0}", isbn);
        }
    }
}