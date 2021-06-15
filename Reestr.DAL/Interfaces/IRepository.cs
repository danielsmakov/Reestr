using Reestr.DAL.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reestr.DAL.Interfaces
{
    public interface IRepository<T> where T : class
    {
        T Get(int id);
        List<T> List(IQuery query);
        bool Insert(T entity);
        bool Update(T entity);
        bool Delete(int id);
    }
}
 