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
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class AuthController : ApiController
    {
        AuthService db = AuthService.instantiateDB();

        [HttpPost]
        [Route("auth/login")]
        public HttpResponseMessage login(User user)
        {
            try
            {
                User data = db.login(user);
                return Request.CreateResponse(HttpStatusCode.OK, data);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(e);
            }
        }

        [HttpPost]
        [Route("auth/register")]
        public HttpResponseMessage register(User user)
        {
            try
            {
                bool data = db.register(user);
                return Request.CreateResponse(HttpStatusCode.OK, data);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(e);
            }
        }
        [HttpGet]
        [Route("auth/validate-key")]
        public HttpResponseMessage Validate(string key)
        {
            try
            {
                bool data = db.isValidServiceKey(key);
                return Request.CreateResponse(HttpStatusCode.OK, data);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(e);
            }
        }
        [HttpPost]
        [Route("user/cart")]
        public HttpResponseMessage ModifyCart(string userID, string item, string operation)
        {
            try
            {
                bool data = db.ModifyUserCart(userID, item, operation);
                return Request.CreateResponse(HttpStatusCode.OK, data);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(e);
            }
        }

        [HttpPost]
        [Route("user/wishlist")]
        public HttpResponseMessage ModifyWishlist(string userID, string item, string operation)
        {
            try
            {
                bool data = db.ModifyUserWishlist(userID, item, operation);
                return Request.CreateResponse(HttpStatusCode.OK, data);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(e);
            }
        }


    }
}