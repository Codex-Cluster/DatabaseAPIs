using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DatabaseAPIs.Models;

namespace DatabaseAPIs.Interfaces
{
    interface IOrder
    {
        bool MakeOrder(Order order);
        bool CancelOrder(Order order);

        List<Order> GetOrders(string userID);
    }
}
