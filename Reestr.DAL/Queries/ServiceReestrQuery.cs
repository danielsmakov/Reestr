using Reestr.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reestr.DAL.Queries
{
    public class ServiceReestrQuery : PaginationSortingQuery, IQuery
    {
        public string OrganizationName { get; set; }

        public string ServiceName { get; set; }

        public bool IsDeleted { get; set; }
    }
}
