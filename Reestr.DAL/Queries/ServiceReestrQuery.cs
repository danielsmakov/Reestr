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
        public string OrganizationNameToSearchFor { get; set; }

        public string ServiceNameToSearchFor { get; set; }

        public bool IsDeleted { get; set; }
    }
}
