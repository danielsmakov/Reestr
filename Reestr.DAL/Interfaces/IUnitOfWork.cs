using Reestr.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reestr.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Organization> Organizations { get; }
        IRepository<Service> Services { get; }
        IRepository<ServiceReestr> ServicesReestres { get; }
        IRepository<EmployeeReestr> EmployeeReestres { get; }
        IRepository<ProducedService> ProducedServices { get; }
    }
}
