using Reestr.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reestr.DAL.Queries
{
    public class EmployeeReestrQuery : PaginationSortingQuery, IQuery
    {
        public int Id { get; set; }

        public string OrganizationName { get; set; }

        public string IIN { get; set; }

        public string FullName { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime DateOfBirth { get; set; }

        public bool IsDeleted { get; set; }
    }
}
