using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data.SqlClient;
using System.Configuration;
using System.Text;
using System.Security.Cryptography;

using DatabaseAPIs.Models;
using DatabaseAPIs.Interfaces;

namespace DatabaseAPIs.Services
{
    public class AdminControls : IAdmin
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["CodexDB"].ConnectionString;

        private static AdminControls Instance = null;
        public static AdminControls instantiateDB()
        {
            if (Instance == null)
            {
                Instance = new AdminControls();
            }
            return Instance;
        }
        public bool AddCoupon(Coupon coupon)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandText = String.Format(
                    "exec AddCouponCode @code = '{0}', @status = '{1}', @creator = '{2}', @discount = '{3}'",
                    coupon.code, coupon.status, coupon.creator, coupon.discount
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

        public bool ModifyCoupon(Coupon coupon)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandText = String.Format(
                    "exec ModifyCoupon @code = '{0}', @status = '{1}', @creator = '{2}', @discount = '{3}', @id = '{4}'",
                    coupon.code, coupon.status, coupon.creator, coupon.discount, coupon.id
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

        public bool RemoveCoupon(Coupon coupon)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandText = String.Format(
                    "exec RemoveCoupon @id = '{0}'",
                    coupon.id
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


        public bool SetAuthorized(string userID, bool status)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandText = String.Format(
                    "update Users set Active = '{0}' where UserID = '{1}'",
                    status, userID
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

        public List<User> getUserList(string userID)
        {
            string roles;
            List<string> roleList = new List<string>();
            List<User> userList = new List<User>();
            bool flag = false;
            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandText = String.Format(
                    "select Roles from Users where UserID = '{0}'",
                    userID
                );
                con.Open();
                try
                {
                    SqlDataReader dr = cmd.ExecuteReader();
                    dr.Read();
                    roles = dr.GetString(0);
                    roleList = roles.Split(',').ToList();
                    foreach( string role in roleList)
                    {
                        if( role == "Admin")
                        {
                            flag = true;
                            break;
                        }
                    }
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }
            if(flag == true)
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = String.Format(
                        "select Name, Email, Active, Roles from Users"
                    );
                    con.Open();
                    try
                    {
                        
                        SqlDataReader dr = cmd.ExecuteReader();
                        while (dr.Read())
                        {
                            userList.Add(new User() { 
                                Name = dr["Name"].ToString(),
                                Email = dr["Email"].ToString(),
                                Active = bool.Parse(dr["Active"].ToString()),
                                Roles = dr["Roles"].ToString().Split(',').ToList()
                            });
                        }
                    }
                    catch (Exception e)
                    {
                        throw new Exception(e.Message);
                    }
                }
            }
            else
            {
                throw new Exception("Failed to authenticate Admin");
            }
            return userList;
        }
        public List<Coupon> CouponList(string userID)
        {
            string roles;
            List<string> roleList = new List<string>();
            List<Coupon> couponList = new List<Coupon>();
            bool flag = false;
            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandText = String.Format(
                    "select Roles from Users where UserID = '{0}'",
                    userID
                );
                con.Open();
                try
                {
                    SqlDataReader dr = cmd.ExecuteReader();
                    dr.Read();
                    roles = dr.GetString(0);
                    roleList = roles.Split(',').ToList();
                    foreach (string role in roleList)
                    {
                        if (role == "Admin")
                        {
                            flag = true;
                            break;
                        }
                    }
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }
            if (flag == true)
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = String.Format(
                        "select id, code, discount, [status], creator from Users"
                    );
                    con.Open();
                    try
                    {

                        SqlDataReader dr = cmd.ExecuteReader();
                        while (dr.Read())
                        {
                            couponList.Add(new Coupon()
                            {
                                id = dr["id"].ToString(),
                                code = dr["code"].ToString(),
                                discount = float.Parse(dr["discount"].ToString()),
                                status = dr["status"].ToString(),
                                creator = dr["creator"].ToString()
                            }) ;
                        }
                    }
                    catch (Exception e)
                    {
                        throw new Exception(e.Message);
                    }
                }
            }
            else
            {
                throw new Exception("Failed to authenticate Admin");
            }
            return couponList;
        }
    }
}