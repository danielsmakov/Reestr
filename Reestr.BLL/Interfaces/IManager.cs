using Reestr.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reestr.BLL.Interfaces
{
    public interface IManager<T> where T : class
    {
        T Get(int id);
        List<T> List(IQuery query);
        void Insert(T DTO);
        void Update(T DTO);
        void Delete(int id);
        void Dispose();
    }
}
