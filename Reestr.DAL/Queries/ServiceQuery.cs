using Reestr.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reestr.DAL.Queries
{
    public class ServiceQuery : PaginationQuery, IQuery
    {
        public string Name { get; set; }

        public bool IsDeleted { get; set; }
    }
}
