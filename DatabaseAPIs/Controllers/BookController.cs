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
    public class BookController : ApiController
    {
        BookService db = BookService.instantiateDB();

        [HttpGet]
        public HttpResponseMessage GetBooks()
        {
            try
            {
                List<Book> data = db.GetData();
                return Request.CreateResponse(HttpStatusCode.OK, data);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.NoContent);
            }
        }
        public HttpResponseMessage GetBooks(string bookID)
        {
            try
            {
                Book data = db.GetData(bookID);
                return Request.CreateResponse(HttpStatusCode.OK, data);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
        }

        [HttpPut]
        public HttpResponseMessage PutData(Book book)
        {
            try
            {
                string data = db.PutData(book);
                return Request.CreateResponse(HttpStatusCode.OK, data);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.NotModified);
            }

        }


        [HttpPost]
        public HttpResponseMessage PostBook(Book book)
        {
            try
            {
                string data = db.PostData(book);
                return Request.CreateResponse(HttpStatusCode.OK, data);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.Conflict);
            }
        }
        [HttpDelete]
        public HttpResponseMessage DeleteBook(string bookID)
        {
            try
            {
                string data = db.DeleteData(bookID);
                return Request.CreateResponse(HttpStatusCode.OK, data);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.Forbidden);
            }
        }
    }
}