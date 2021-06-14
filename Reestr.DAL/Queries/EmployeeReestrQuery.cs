using Reestr.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reestr.DAL.Queries
{
    public class EmployeeReestrQuery : PaginationQuery, IQuery
    {
        public string OrganizationName { get; set; }

        public string FullName { get; set; }

        public bool IsDeleted { get; set; }
    }
}
