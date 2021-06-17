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
    public static class AutoMapperConfigurator
    {
        public static IMapper GetMapper()
        {
            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Organization, OrganizationDTO>();
                cfg.CreateMap<OrganizationDTO, Organization>();

                cfg.CreateMap<Service, ServiceDTO>();
                cfg.CreateMap<ServiceDTO, Service>();

                cfg.CreateMap<ServiceReestr, ServiceReestrDTO>();
                cfg.CreateMap<ServiceReestrDTO, ServiceReestr>();

                cfg.CreateMap<EmployeeReestr, EmployeeReestrDTO>();
                cfg.CreateMap<EmployeeReestrDTO, EmployeeReestr>();

                cfg.CreateMap<ProducedService, ProducedServiceDTO>();
                cfg.CreateMap<ProducedServiceDTO, ProducedService>();
            }).CreateMapper();
            return mapper;
        }
    }
}
