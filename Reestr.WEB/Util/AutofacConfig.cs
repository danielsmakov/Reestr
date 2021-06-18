using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Reestr.BLL.Validation;
using Reestr.DAL.Entities;
using Reestr.DAL.Interfaces;
using Reestr.DAL.Repositories;

namespace Reestr.WEB.Util
{
    public class AutofacConfig
    {
        public static void ConfigureContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();

            builder.RegisterType<OrganizationRepository>().As<IRepository<Organization>>();
            builder.RegisterType<ServiceRepository>().As<IRepository<Service>>();
            builder.RegisterType<ServiceReestrRepository>().As<IRepository<ServiceReestr>>();
            builder.RegisterType<EmployeeReestrRepository>().As<IRepository<EmployeeReestr>>();
            builder.RegisterType<ProducedServiceRepository>().As<IRepository<ProducedService>>();

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}