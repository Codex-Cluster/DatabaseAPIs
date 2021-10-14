using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DatabaseAPIs.Models;

namespace DatabaseAPIs.Interfaces
{
    interface ICategory
    {
        List<string> GetCategories();
        List<Book> GetSubCategories(string CatID);
    }
}
