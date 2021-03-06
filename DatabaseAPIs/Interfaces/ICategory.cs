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
        List<Category> GetCategories();
        List<Book> GetBooks(string CatID);

        Category AddCategory(Category category);
        bool DeleteCategory(Category category);
    }
}
