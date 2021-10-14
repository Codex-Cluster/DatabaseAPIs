using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DatabaseAPIs.Models;

namespace DatabaseAPIs.Interfaces
{
    interface IBook
    {
        List<Book> GetData();
        Book GetData(string bookID);
        string PutData(Book book);

        string PostData(Book book);
        string DeleteData(string isbn);
    }
}
