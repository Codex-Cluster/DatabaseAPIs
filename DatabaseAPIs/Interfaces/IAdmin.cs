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

        User modifyUser(User user);

        List<User> getUserList(string userID);
    }
}
