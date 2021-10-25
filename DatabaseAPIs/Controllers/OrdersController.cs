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
    public class OrdersController : ApiController
    {
        OrderService db = OrderService.instantiateDB();

        [HttpPost]
        public HttpResponseMessage makeOrder(Order order)
        {
            try
            {
                bool data = db.MakeOrder(order);
                return Request.CreateResponse(HttpStatusCode.OK, data);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(e);
            }
        }

        [HttpDelete]
        public HttpResponseMessage cancelOrder(Order order)
        {
            try
            {
                bool data = db.CancelOrder(order);
                return Request.CreateResponse(HttpStatusCode.OK, data);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(e);
            }
        }
        [HttpGet]
        public HttpResponseMessage getAllOrders(string userID)
        {
            try
            {
                OrderDetails data = db.GetOrders(userID);
                return Request.CreateResponse(HttpStatusCode.OK, data);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(e);
            }
        }
    }
}