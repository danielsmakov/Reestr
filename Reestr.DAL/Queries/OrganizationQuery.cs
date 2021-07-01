using Reestr.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reestr.DAL.Queries
{
    public class OrganizationQuery : PaginationSortingQuery, IQuery 
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string NameToSearchFor { get; set; }

        public string BIN { get; set; }

        public bool IsDeleted { get; set; }
    }
}
