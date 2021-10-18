﻿using System;
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
    public class UserController : ApiController
    {
        UserDataService db = UserDataService.instantiateDB();

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