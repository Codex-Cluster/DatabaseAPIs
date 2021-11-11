using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DatabaseAPIs.Models;

namespace DatabaseAPIs.Interfaces
{
    interface IPromoted
    {
        List<Promoted> GetPromoted();
        bool PostPromoted(Promoted p);
        bool PutPromoted(Promoted p);
        bool DeletePromoted(Promoted p);

    }
}
