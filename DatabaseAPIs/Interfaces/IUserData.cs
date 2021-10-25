using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DatabaseAPIs.Models;

namespace DatabaseAPIs.Interfaces
{
    public interface IUserData
    { 
        bool ModifyUserCart(string userID, string item, string operation, int qty);
        bool ModifyUserWishlist(string userID, string item, string operation, int qty);

        bool UpdateUserInfo(User user);

        List<Order> GetCart(string userID);
        List<Order> GetWishlist(string userID);

        bool MoveToCart(string userID, string item, int qty);
        bool MoveToWishlist(string userID, string item, int qty);
    }
}