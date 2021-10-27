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
    public class UserDataService : IUserData
    {
        private static UserDataService Instance = null;
        public static UserDataService instantiateDB()
        {
            if (Instance == null)
            {
                Instance = new UserDataService();
            }
            return Instance;
        }

        private string connectionString = ConfigurationManager.ConnectionStrings["CodexDB"].ConnectionString;
        public bool ModifyUserCart(string userID, string item, string operation, int qty = 1)
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
                        cartDict[item] = qty;
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
                                cartDict[item] += qty;
                            }
                            else if (operation == "Remove")
                            {
                                if ((cartDict[item] - qty) <= 1)
                                {
                                    cartDict.Remove(item);
                                }
                                else
                                {
                                    cartDict[item] -= qty;
                                }
                            }
                            else if (operation == "Delete")
                            {
                                cartDict.Remove(item);
                            }
                        }
                        if (flag != true && operation == "Add")
                        {
                            cartDict[item] = qty;
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
            foreach (string key in cartDict.Keys)
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
        public bool ModifyUserWishlist(string userID, string item, string operation, int qty = 1)
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
                        wishlistDict[item] = qty;
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
                            if (operation == "Add")
                            {
                                wishlistDict[item] += qty;
                            }
                            else if (operation == "Remove")
                            {
                                if ((wishlistDict[item]-qty) <= 0)
                                {
                                    wishlistDict.Remove(item);
                                }
                                else
                                {
                                    wishlistDict[item] -= qty;
                                }
                            }
                            else if (operation == "Delete")
                            {
                                wishlistDict.Remove(item);
                            }

                        }

                        if (flag != true && operation == "Add")
                        {
                            wishlistDict[item] = qty;
                        }
                        else if (flag != true && operation != "Add")
                        {
                            throw new Exception("Item does not exist in wishlist");
                        }
                    }

                }
                catch (Exception e)
                {
                    throw new Exception("Failed to retrieve wishlist from database", e);
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

        public User UpdateUserInfo(User user)
        {
            string roles = string.Join("+", user.Roles);
            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandText = String.Format(
                    "exec ModifyUserInfo @userID = '{0}', @name = '{1}', @email = '{2}', @mobile = '{3}', @address = '{4}', @roles = '{5}', @active = '{6}'",
                    user.UserID, user.Name, user.Email, user.Mobile, user.Address, roles, user.Active
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
            return GetUserData(user.UserID);
        }
        private User GetUserData(string userID)
        {
            User user = new User();
            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandText = String.Format(
                    "select * from Users where UserID = '{0}'",
                    userID
                    );
                con.Open();
                try
                {
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        user.Name = dr["Name"].ToString().Trim(' ');
                        user.Email = dr["Email"].ToString();
                        user.Mobile = dr["Mobile"].ToString();
                        user.UserID = dr["UserID"].ToString().Trim(' ');
                        user.Password = dr["Password"].ToString();
                        user.Active = Boolean.Parse(dr["Active"].ToString());
                        user.Cart = dr["Cart"].ToString().Split('+').ToList<string>();
                        user.Roles = dr["Roles"].ToString().Split('+').ToList<string>();
                        user.Wishlist = dr["Wishlist"].ToString().Split('+').ToList<string>();
                        user.Address = dr["Address"].ToString();
                    }
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }

            }
            return user;
        }

        public List<Order> GetCart(string userID)
        {
            List<Order> books = new List<Order>();
            string cart = string.Empty;
            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandText = String.Format(
                    "select Cart from Users where UserID='{0}'",
                    userID
                    );
                con.Open();
                try
                {
                    SqlDataReader dr = cmd.ExecuteReader();
                    dr.Read();
                    cart = dr.GetString(0);
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }
            List<string> bookList = new List<string>();
            bookList = cart.Split('+').ToList();
            foreach (string bookInfo in bookList)
            {
                string bookID = bookInfo.Split(':').ToList()[0];
                using (SqlConnection con = new SqlConnection(connectionString))
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = String.Format(
                        "select * from Books where BookID='{0}'",
                        bookID
                        );
                    con.Open();
                    try
                    {
                        SqlDataReader dr = cmd.ExecuteReader();
                        while (dr.Read())
                        {
                            books.Add(new Order()
                            {
                                BookID = dr["BookID"].ToString().Trim(' '),
                                CatID = dr["CatID"].ToString(),
                                Image = dr["Image"].ToString(),
                                Author = dr["Author"].ToString(),
                                Title = dr["Title"].ToString(),
                                Format = dr["Format"].ToString(),
                                Rating = double.Parse(dr["Rating"].ToString()),
                                Price = double.Parse(dr["Price"].ToString()),
                                OldPrice = double.Parse(dr["OldPrice"].ToString()),
                                ISBN = dr["ISBN"].ToString(),
                                Description = dr["Description"].ToString(),
                                Year = dr["Year"].ToString(),
                                Position = Int32.Parse(dr["Position"].ToString()),
                                Status = dr["Status"].ToString(),
                                qty = Int32.Parse(bookInfo.Split(':').ToList()[1])
                            });
                        }
                    }
                    catch (Exception e)
                    {
                        throw new Exception(e.Message);
                    }

                }
            }

            return books;
        }
        public List<Order> GetWishlist(string userID)
        {
            List<Order> books = new List<Order>();
            string wishlist = string.Empty;
            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandText = String.Format(
                    "select Wishlist from Users where UserID='{0}'",
                    userID
                    );
                con.Open();
                try
                {
                    SqlDataReader dr = cmd.ExecuteReader();
                    dr.Read();
                    wishlist = dr.GetString(0);
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }
            List<string> bookList = new List<string>();
            if (wishlist.Contains('+'))
            {
                bookList = wishlist.Split('+').ToList();
            }
            else
            {
                bookList.Add(wishlist);
            }
            
            foreach(string bookInfo in bookList)
            {
                string bookID = bookInfo.Split(':').ToList()[0];
                using (SqlConnection con = new SqlConnection(connectionString))
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = String.Format(
                        "select * from Books where BookID='{0}'",
                        bookID
                        );
                    con.Open();
                    try
                    {
                        SqlDataReader dr = cmd.ExecuteReader();
                        while (dr.Read())
                        {
                            books.Add(new Order()
                            {
                                BookID = dr["BookID"].ToString().Trim(' '),
                                CatID = dr["CatID"].ToString(),
                                Image = dr["Image"].ToString(),
                                Author = dr["Author"].ToString(),
                                Title = dr["Title"].ToString(),
                                Format = dr["Format"].ToString(),
                                Rating = double.Parse(dr["Rating"].ToString()),
                                Price = double.Parse(dr["Price"].ToString()),
                                OldPrice = double.Parse(dr["OldPrice"].ToString()),
                                ISBN = dr["ISBN"].ToString(),
                                Description = dr["Description"].ToString(),
                                Year = dr["Year"].ToString(),
                                Position = Int32.Parse(dr["Position"].ToString()),
                                Status = dr["Status"].ToString(),
                                qty = Int32.Parse(bookInfo.Split(':').ToList()[1])
                            });
                        }
                    }
                    catch (Exception e)
                    {
                        throw new Exception(e.Message);
                    }

                }
            }

            return books;
        }
        public bool MoveToCart(string userID, string item, int qty)
        {
            try
            {
                this.ModifyUserWishlist(userID, item, "Remove", qty);
                this.ModifyUserCart(userID, item, "Add", qty);
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
            return true;
        }
        public bool MoveToWishlist(string userID, string item, int qty)
        {
            try
            {
                this.ModifyUserCart(userID, item, "Remove", qty);
                this.ModifyUserWishlist(userID, item, "Add", qty);

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return true;
        }
    }
}