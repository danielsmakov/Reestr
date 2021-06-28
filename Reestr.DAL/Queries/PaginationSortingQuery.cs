using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reestr.DAL.Queries
{
    public abstract class PaginationSortingQuery
    {
        public int Offset { get; set; }

        public int Limit { get; set; } = 20;

        /*public string FieldToSortBy { get; set; }

        public string DirectionOfSorting { get; set; }*/

        public Dictionary<string, string>[] SortingParameters { get; set; }
        
}

}
