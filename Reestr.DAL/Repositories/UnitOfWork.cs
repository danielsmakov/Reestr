using Reestr.DAL.Entities;
using Reestr.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reestr.DAL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["RegistryDBConnection"].ConnectionString;
        private IDbConnection db = new SqlConnection(connectionString);
        private OrganizationRepository organizationRepository;
        private ServiceRepository serviceRepository;
        private ServiceReestrRepository serviceReestrRepository;
        private EmployeeReestrRepository employeeReestrRepository;
        private ProducedServiceRepository producedServiceRepository;
        public IRepository<Organization> Organizations
        {
            get
            {
                if (organizationRepository == null)
                    organizationRepository = new OrganizationRepository();
                return organizationRepository;
            }
        }
        public IRepository<Service> Services
        {
            get
            {
                if (serviceRepository == null)
                    serviceRepository = new ServiceRepository();
                return serviceRepository;
            }
        }
        public IRepository<ServiceReestr> ServiceReestres
        {
            get
            {
                if (serviceReestrRepository == null)
                    serviceReestrRepository = new ServiceReestrRepository();
                return serviceReestrRepository;
            }
        }
        public IRepository<EmployeeReestr> EmployeeReestres
        {
            get
            {
                if (employeeReestrRepository == null)
                    employeeReestrRepository = new EmployeeReestrRepository();
                return employeeReestrRepository;
            }
        }
        public IRepository<ProducedService> ProducedServices
        {
            get
            {
                if (producedServiceRepository == null)
                    producedServiceRepository = new ProducedServiceRepository();
                return producedServiceRepository;
            }
        }
        private bool disposed = false;
        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
                this.disposed = true;
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
