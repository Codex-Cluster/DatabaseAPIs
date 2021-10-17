using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DatabaseAPIs.Models;

namespace DatabaseAPIs.Interfaces
{
    interface IAuth
    {
        User login(User user);
        bool register(User user);
        bool isValidServiceKey(string key);

        bool ModifyUserCart(string userID, string item, string operation);
        bool ModifyUserWishlist(string userID, string item, string operation);
    }
}
