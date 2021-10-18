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
            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandText = String.Format(
                    "exec CancelOrder @orderID = '{0}', @userId = '{1}'",
                    order.orderID, order.userID
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

        public List<Order> GetOrders(string userID)
        {

            List<Order> orderList = new List<Order>();
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
                        orderList.Add(
                            new Order()
                            {
                                books = dr["books"].ToString(),
                                datetime = dr["datetime"].ToString(),
                                amt = float.Parse(dr["AmountPaid"].ToString()),
                                address = dr["Address"].ToString()
                            }
                            );
                    }
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }

            }
            return orderList;
        }

        public bool MakeOrder(Order order)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandText = String.Format(
                    "exec MakeOrder @userID = '{0}', @books = '{1}', @coupon = '{2}', @amt = '{3}', @address = '{4}'",
                    order.userID, order.books, order.coupon, order.amt, order.address
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