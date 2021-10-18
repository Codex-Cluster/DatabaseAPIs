using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using System.Web.Http.Cors;

using DatabaseAPIs.Models;
using DatabaseAPIs.Services;

namespace DatabaseAPIs.Controllers
{
    public class AdminController : ApiController
    {

        AdminControls db = AdminControls.instantiateDB();

        [HttpPost]
        [Route("admin/authorize")]
        public HttpResponseMessage authorize(string user, bool status)
        {
            try
            {
                bool data = db.SetAuthorized(user, status);
                return Request.CreateResponse(HttpStatusCode.OK, data);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(e);
            }
        }

        [HttpPost]
        [Route("admin/coupon-add")]
        public HttpResponseMessage addCoupon(Coupon coupon)
        {
            try
            {
                bool data = db.AddCoupon(coupon);
                return Request.CreateResponse(HttpStatusCode.OK, data);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(e);
            }
        }
        [HttpPost]
        [Route("admin/coupon-modify")]
        public HttpResponseMessage modifyCoupon(Coupon coupon)
        {
            try
            {
                bool data = db.ModifyCoupon(coupon);
                return Request.CreateResponse(HttpStatusCode.OK, data);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(e);
            }
        }
        [HttpDelete]
        [Route("admin/coupon")]
        public HttpResponseMessage removeCoupon(Coupon coupon)
        {
            try
            {
                bool data = db.RemoveCoupon(coupon);
                return Request.CreateResponse(HttpStatusCode.OK, data);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(e);
            }
        }

        [HttpGet]
        [Route("admin/coupon")]
        public HttpResponseMessage CouponList(string userID)
        {
            try
            {
                List<Coupon> data = db.CouponList(userID);
                return Request.CreateResponse(HttpStatusCode.OK, data);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(e);
            }
        }
    }
}