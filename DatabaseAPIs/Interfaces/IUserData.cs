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
        bool ModifyUserCart(string userID, string item, string operation);
        bool ModifyUserWishlist(string userID, string item, string operation);

        bool UpdateUserInfo(User user);
    }
}