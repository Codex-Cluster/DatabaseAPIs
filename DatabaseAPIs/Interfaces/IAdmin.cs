using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DatabaseAPIs.Models;

namespace DatabaseAPIs.Interfaces
{
    interface IAdmin
    {
        List<Coupon> CouponList(string userID);
        bool AddCoupon(Coupon coupon);

        bool RemoveCoupon(Coupon coupon);
        bool ModifyCoupon(Coupon coupon);

        bool SetAuthorized(string userID, bool status);

        List<User> getUserList(string userID);
    }
}
