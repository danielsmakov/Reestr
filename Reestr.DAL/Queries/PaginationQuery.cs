using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reestr.DAL.Queries
{
    public abstract class PaginationQuery
    {
        public int Offset { get; set; }

        public int Limit { get; set; } = 20;
    }
}
