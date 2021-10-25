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
    public class AuthService : IAuth
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["CodexDB"].ConnectionString;

        private static AuthService Instance = null;
        public static AuthService instantiateDB()
        {
            if (Instance == null)
            {
                Instance = new AuthService();
            }
            return Instance;
        }
        private int count;
        private AuthService()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandText = String.Format("exec LastUserID");
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                dr.Read();
                count = dr.GetInt32(0);
            }
        }

        public User login(User user)
        {
            string pswdHashed = ComputeSha256Hash(user.Password);
            string pswdFromDB = String.Empty;
            User ResUser = new User();
            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandText = String.Format("select Password from Users where Email='{0}'", user.Email);
                con.Open();
                try
                {
                    SqlDataReader dr = cmd.ExecuteReader();
                    dr.Read();
                    pswdFromDB = dr.GetString(0);
                }
                catch (Exception e)
                {
                    throw new Exception("User does not exist in database");
                }
                con.Close();

                if (pswdFromDB == pswdHashed)
                {
                    cmd.CommandText = String.Format("select * from Users where Email='{0}'", user.Email);
                    con.Open();
                    try
                    {
                        SqlDataReader dr = cmd.ExecuteReader();
                        while (dr.Read())
                        {
                            ResUser.Name = dr["Name"].ToString().Trim(' ');
                            ResUser.Email = dr["Email"].ToString();
                            ResUser.Mobile = Int32.Parse(dr["Mobile"].ToString());
                            ResUser.UserID = dr["UserID"].ToString().Trim(' ');
                            ResUser.Password = dr["Password"].ToString();
                            ResUser.Active = Boolean.Parse(dr["Active"].ToString());
                            ResUser.Cart = dr["Cart"].ToString().Split('+').ToList<string>();
                            ResUser.Roles = dr["Roles"].ToString().Split('+').ToList<string>();
                            ResUser.Wishlist = dr["Wishlist"].ToString().Split('+').ToList<string>();
                            ResUser.Address = dr["Address"].ToString();
                        }
                        return ResUser;
                    }
                    catch (Exception e)
                    {
                        throw new Exception(e.Message);
                    }
                }
                else
                {
                    throw new Exception("Passwords do not match");
                }
            }

        }

        public User register(User user)
        {
            count++;
            user.UserID = "user_" + new string('0', (5 - count.ToString().Length)) + count.ToString();
            user.Password = ComputeSha256Hash(user.Password);
            string roles = string.Join("+", user.Roles.ToArray());
            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandText = String.Format(
                    "insert into Users (userID, Name, Email, Mobile, Password, Roles, Address) values ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}')",
                    user.UserID, user.Name, user.Email, user.Mobile, user.Password, roles, user.Address
                    );
                con.Open();
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }

            }
            return user;
        }

        static string ComputeSha256Hash(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public bool isValidServiceKey(string key)
        {
            bool ResDB = false;
            using(SqlConnection con = new SqlConnection(connectionString))
            using(SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandText = String.Format("execute CheckIfServiceKeyExists @key = '{0}'", key);
                con.Open();
                try
                {
                    SqlDataReader dr = cmd.ExecuteReader();
                    dr.Read();
                    ResDB = bool.Parse(dr.GetString(0));
                }
                
                catch (Exception e)
                {
                    throw new Exception("Failed to check service key validity from database\n{0}",e);
                }
                return ResDB;
            }
        }
    }
}