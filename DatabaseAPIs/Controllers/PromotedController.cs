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
    public class PromotedController : ApiController
    {
        PromotedService db = PromotedService.instantiateDB();


        [HttpGet]
        public HttpResponseMessage GetPromoted()
        {
            try
            {
                List<Promoted> data = db.GetPromoted();
                return Request.CreateResponse(HttpStatusCode.OK, data);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(e);
            }
        }
        [HttpPost]
        public HttpResponseMessage PostPromoted(Promoted p)
        {
            try
            {
                bool data = db.PostPromoted(p);
                return Request.CreateResponse(HttpStatusCode.OK, data);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(e);
            }
        }
        [HttpPut]
        public HttpResponseMessage PutPromoted(Promoted p)
        {
            try
            {
                bool data = db.PutPromoted(p);
                return Request.CreateResponse(HttpStatusCode.OK, data);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(e);
            }
        }
        [HttpDelete]
        public HttpResponseMessage DeletePromoted(Promoted p)
        {
            try
            {
                bool data = db.DeletePromoted(p);
                return Request.CreateResponse(HttpStatusCode.OK, data);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(e);
            }
        }
    }
}