using AutoMapper;
using Reestr.BLL.DTOs;
using Reestr.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reestr.BLL
{
    public class AutoMapperConfigurator
    {
        public IMapper Mapper { get; }
        public AutoMapperConfigurator()
        {
            Mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Organization, OrganizationDTO>();
                cfg.CreateMap<OrganizationDTO, Organization>();
                cfg.CreateMap<List<Organization>, List<OrganizationDTO>>();
                cfg.CreateMap<List<OrganizationDTO>, List<Organization>>();

                cfg.CreateMap<Service, ServiceDTO>();
                cfg.CreateMap<ServiceDTO, Service>();
                cfg.CreateMap<List<Service>, List<ServiceDTO>>();
                cfg.CreateMap<List<ServiceDTO>, List<Service>>();

                cfg.CreateMap<ServiceReestr, ServiceReestrDTO>();
                cfg.CreateMap<ServiceReestrDTO, ServiceReestr>();
                cfg.CreateMap<List<ServiceReestr>, List<ServiceReestrDTO>>();
                cfg.CreateMap<List<ServiceReestrDTO>, List<ServiceReestr>>();

                cfg.CreateMap<EmployeeReestr, EmployeeReestrDTO>();
                cfg.CreateMap<EmployeeReestrDTO, EmployeeReestr>();
                cfg.CreateMap<List<EmployeeReestr>, List<EmployeeReestrDTO>>();
                cfg.CreateMap<List<EmployeeReestrDTO>, List<EmployeeReestr>>();

                cfg.CreateMap<ProducedService, ProducedServiceDTO>();
                cfg.CreateMap<ProducedServiceDTO, ProducedService>();
                cfg.CreateMap<List<ProducedService>, List<ProducedServiceDTO>>();
                cfg.CreateMap<List<ProducedServiceDTO>, List<ProducedService>>();
            }).CreateMapper();
        }
    }
}
