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
                cmd.CommandText = String.Format("select count(*) from Users");
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
                    throw new Exception("Failed to retrieve hashed password from database\n",e);
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
                            ResUser.Cart = dr["Cart"].ToString().Split(';').ToList<string>();
                            ResUser.Roles = dr["Roles"].ToString().Split(';').ToList<string>();
                            ResUser.Wishlist = dr["Wishlist"].ToString().Split(';').ToList<string>();
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

        public bool register(User user)
        {
            user.UserID = "user_" + count;
            count++;
            int rows = 0;
            user.Password = ComputeSha256Hash(user.Password);
            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandText = String.Format(
                    "insert into Users (userID, Name, Email, Mobile, Password, Roles, Address) values ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}')",
                    user.UserID, user.Name, user.Email, user.Mobile, user.Password, user.Roles, user.Address
                    );
                con.Open();
                try
                {
                    rows = cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    return false;
                }

            }
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return true;
            }
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

        public bool ModifyUserCart(string userID, string item, string operation)
        {
            string cart = "";
            List<string> cartList = new List<string>();
            Dictionary<string, int> cartDict = new Dictionary<string, int>();
            bool flag = false;
            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandText = String.Format("execute GetUserCart @userID = '{0}'", userID);
                con.Open();
                try
                {
                    SqlDataReader dr = cmd.ExecuteReader();
                    dr.Read();
                    cart = dr.GetString(0);
                    if (cart.Count() == 0)
                    {
                        cartDict[item] = 1;
                    }
                    else
                    {
                        cartList = cart.Split('+').ToList<string>();
                        foreach (string val in cartList)
                        {
                            var tmpItem = val.Split(':').ToList();
                            cartDict[tmpItem[0]] = Int32.Parse(tmpItem[1]);

                        }
                        if (cartDict.Keys.Contains(item))
                        {
                            flag = true;
                            if (operation == "Add")
                            {
                                cartDict[item] += 1;
                            }
                            else if (operation == "Remove")
                            {
                                if (cartDict[item] == 1)
                                {
                                    cartDict.Remove(item);
                                }
                                else
                                {
                                    cartDict[item] -= 1;
                                }
                            }
                            else if (operation == "Delete")
                            {
                                cartDict.Remove(item);
                            }
                        }
                        if (flag != true && operation == "Add")
                        {
                            cartDict[item] = 1;
                        }
                        else if (flag != true && operation != "Add")
                        {
                            throw new Exception("Item does not exist in cart");
                        }
                    }
                    
                }
                catch (Exception e)
                {
                    throw new Exception("Failed to retrieve cart from database", e);
                }
            }
            cartList = new List<string>();
            foreach(string key in cartDict.Keys)
            {
                var tmpItem = key + ':' + cartDict[key].ToString();
                cartList.Add(tmpItem);
            }
            cart = "";
            cart = string.Join("+", cartList);

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandText = String.Format("execute ModifyUserCart @item = '{0}', @userID = '{1}'", cart, userID);
                con.Open();
                try
                {
                    cmd.ExecuteNonQuery();
                }

                catch (Exception e)
                {
                    throw new Exception("Failed to add item to cart within database", e);
                }
                return true;
            }
        }
        public bool ModifyUserWishlist(string userID, string item, string operation)
        {
            string wishlist = "";
            List<string> wishlistList = new List<string>();
            Dictionary<string, int> wishlistDict = new Dictionary<string, int>();
            bool flag = false;
            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandText = String.Format("execute GetUserWishlist @userID = '{0}'", userID);
                con.Open();
                try
                {
                    SqlDataReader dr = cmd.ExecuteReader();
                    dr.Read();
                    wishlist = dr.GetString(0);
                    if (wishlist.Count() == 0)
                    {
                        wishlistDict[item] = 1;
                    }
                    else
                    {
                        wishlistList = wishlist.Split('+').ToList<string>();
                        foreach (string val in wishlistList)
                        {
                            var tmpItem = val.Split(':').ToList();
                            wishlistDict[tmpItem[0]] = Int32.Parse(tmpItem[1]);

                        }
                        if (wishlistDict.Keys.Contains(item))
                        {
                            flag = true;
                            if(operation == "Add")
                            {
                                wishlistDict[item] += 1;
                            }
                            else if(operation == "Remove")
                            {
                                if(wishlistDict[item] == 1)
                                {
                                    wishlistDict.Remove(item);
                                }
                                else
                                {
                                    wishlistDict[item] -= 1;
                                }
                            }
                            else if(operation == "Delete")
                            {
                                wishlistDict.Remove(item);
                            }
                            
                        }

                        if (flag != true && operation == "Add")
                        {
                            wishlistDict[item] = 1;
                        }
                        else if(flag != true && operation != "Add")
                        {
                            throw new Exception("Item does not exist in wishlist");
                        }
                    }

                }
                catch (Exception e)
                {
                    throw new Exception("Failed to retrieve wishlist from database\n{0}", e);
                }
            }
            wishlistList = new List<string>();
            foreach (string key in wishlistDict.Keys)
            {
                string tmpItem = key + ':' + wishlistDict[key].ToString();
                wishlistList.Add(tmpItem);
            }
            wishlist = "";
            wishlist = string.Join("+", wishlistList);

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandText = String.Format("execute ModifyUserWishlist @item = '{0}', @userID = '{1}'", wishlist, userID);
                con.Open();
                try
                {
                    cmd.ExecuteNonQuery();
                }

                catch (Exception e)
                {
                    throw new Exception("Failed to add item to wishlist within database", e);
                }
                return true;
            }
        }
    }
}