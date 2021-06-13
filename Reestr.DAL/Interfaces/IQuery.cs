using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reestr.DAL.Interfaces
{
    public interface IQuery
    {
        bool IsDeleted { get; set; }
    }
}
