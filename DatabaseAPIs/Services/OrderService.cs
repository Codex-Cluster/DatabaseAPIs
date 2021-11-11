using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data.SqlClient;
using System.Configuration;

using DatabaseAPIs.Models;
using DatabaseAPIs.Interfaces;

using Newtonsoft.Json;

namespace DatabaseAPIs.Services
{
    public class OrderService : IOrder
    {

        private string connectionString = ConfigurationManager.ConnectionStrings["CodexDB"].ConnectionString;

        private static OrderService Instance = null;
        public static OrderService instantiateDB()
        {
            if (Instance == null)
            {
                Instance = new OrderService();
            }
            return Instance;
        }


        public bool CancelOrder(Order order)
        {
            List<string> bookList = new List<string>();

            string books = string.Empty;
            foreach (string bookID in order.books.Keys)
            {
                bookList.Add(string.Format("{0}:{1}", bookID, order.books[bookID]));
            }
            int count = 0;
            foreach (string bookQty in bookList)
            {
                books += string.Format("{0}", bookQty);
                count += 1;
                if (count != bookList.Count())
                {
                    books += "+";
                }
            }
            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandText = String.Format(
                    "exec CancelOrder @books = '{0}', @userID = '{1}', @datetime = '{2}'",
                    books, order.userID, order.datetime
                );
                con.Open();
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    return false;
                }

            }
            return true;
        }

        public OrderDetails GetOrders(string userID)
        {

            List<Order> orderList = new List<Order>();
            List<Order> books = new List<Order>();
            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandText = String.Format(
                    "exec OrderedBooks @userId = '{0}'",
                    userID
                );
                con.Open();
                try
                {
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        string _books = dr["books"].ToString();
                        List<string> _bookList = _books.Split('+').ToList();
                        Dictionary<string, int> bookDict = new Dictionary<string, int>();
                        foreach(string _bookID in _bookList)
                        {
                            bookDict[_bookID.Split(':').ToList()[0]] = Int32.Parse(_bookID.Split(':').ToList()[1]);
                        }
                        orderList.Add(new Order()
                        {
                            books = bookDict,
                            datetime = dr["datetime"].ToString(),
                            amt = float.Parse(dr["AmountPaid"].ToString()),
                            address = dr["Address"].ToString(),
                        }
                       );
                    }
                }
                catch (Exception e)
                {
                    throw new Exception("Failed to parse ordered books from DB",e);
                }
            }
            
            OrderDetails orderDetails = new OrderDetails();
            orderDetails.n_orders = 0;
            orderDetails.orderDetails = new List<Dictionary<string, Order>>();
            foreach (Order order in orderList)
            {
                orderDetails.n_orders += 1;
                List<string> bookIDs = order.books.Keys.ToList();
                Dictionary<string, Order> bookList = new Dictionary<string, Order>();
                foreach (string id in bookIDs)
                {
                    using (SqlConnection con = new SqlConnection(connectionString))
                    using (SqlCommand cmd = con.CreateCommand())
                    {
                        cmd.CommandText = String.Format(
                            "select * from Books where BookID='{0}'",
                            id
                        );
                        con.Open();
                        try
                        {
                            bookList.Add(id.ToString(), new Order());
                            SqlDataReader dr = cmd.ExecuteReader();
                            while (dr.Read())
                            {
                                bookList[id].BookID = dr["BookID"].ToString().Trim(' ');
                                bookList[id].CatID = dr["CatID"].ToString();
                                bookList[id].Image = dr["Image"].ToString();
                                bookList[id].Author = dr["Author"].ToString();
                                bookList[id].Title = dr["Title"].ToString();
                                bookList[id].Format = dr["Format"].ToString();
                                bookList[id].Rating = double.Parse(dr["Rating"].ToString());
                                bookList[id].Price = double.Parse(dr["Price"].ToString());
                                bookList[id].OldPrice = double.Parse(dr["OldPrice"].ToString());
                                bookList[id].ISBN = dr["ISBN"].ToString();
                                bookList[id].Description = dr["Description"].ToString();
                                bookList[id].Year = dr["Year"].ToString();
                                bookList[id].Position = Int32.Parse(dr["Position"].ToString());
                                bookList[id].Status = dr["Status"].ToString();
                                bookList[id].address = order.address;
                                bookList[id].amt = order.amt;
                                bookList[id].datetime = order.datetime;
                                bookList[id].qty = order.books[id];

                            }
                        }
                        catch (Exception e)
                        {
                            throw new Exception("Error while making booklist dictionary", e);
                        }

                    }
                }
                orderDetails.orderDetails.Add(bookList);
            }

            return orderDetails;
        }

        public bool MakeOrder(Order order)
        {
            List<string> bookList = new List<string>();
 
            string books = string.Empty;
            foreach (string bookID in order.books.Keys)
            {
                bookList.Add(string.Format("{0}:{1}", bookID, order.books[bookID]));
            }
            int count = 0;
            foreach (string bookQty in bookList)
            {
                books += string.Format("{0}",bookQty);
                count += 1;
                if (count != bookList.Count())
                {
                    books += "+";
                }
            }
            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandText = String.Format(
                    "exec MakeOrder @userID = '{0}', @books = '{1}', @coupon = '{2}', @amt = '{3}', @address = '{4}'",
                    order.userID, books, order.coupon, order.amt, order.address
                );
                con.Open();
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    return false;
                }

            }
            return true;
        }
    }
}