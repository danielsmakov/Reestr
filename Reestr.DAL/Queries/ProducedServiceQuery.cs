using Reestr.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reestr.DAL.Queries
{
    public class ProducedServiceQuery : PaginationQuery, IQuery
    {
        public bool IsDeleted { get; set; }
    }
}
