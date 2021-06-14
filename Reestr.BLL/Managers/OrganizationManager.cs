using Reestr.BLL.DTOs;
using Reestr.BLL.Interfaces;
using Reestr.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Reestr.DAL.Entities;

namespace Reestr.BLL.Managers
{
    public class OrganizationManager : IManager<OrganizationDTO>
    {
        private IUnitOfWork Database { get; set; }
        public OrganizationManager(IUnitOfWork unitOfWork)
        {
            Database = unitOfWork;
        }
        public OrganizationDTO Get(int id)
        {
            try
            {
                var organization = Database.Organizations.Get(id);

                var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Organization, OrganizationDTO>()).CreateMapper();
                return mapper.Map<Organization, OrganizationDTO>(organization);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public List<OrganizationDTO> List(IQuery query)
        {
            try
            {
                var organizations = Database.Organizations.List(query);

                var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Organization, OrganizationDTO>()).CreateMapper();
                return mapper.Map<List<Organization>, List<OrganizationDTO>>(organizations);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public void Insert(OrganizationDTO organizationDTO)
        {
            try
            {
                var mapper = new MapperConfiguration(cfg => cfg.CreateMap<OrganizationDTO, Organization>()).CreateMapper();
                var organization = mapper.Map<OrganizationDTO, Organization>(organizationDTO);

                Database.Organizations.Insert(organization);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public void Update(OrganizationDTO organizationDTO)
        {
            try
            {
                var mapper = new MapperConfiguration(cfg => cfg.CreateMap<OrganizationDTO, Organization>()).CreateMapper();
                var organization = mapper.Map<OrganizationDTO, Organization>(organizationDTO);

                Database.Organizations.Update(organization);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Delete(int id)
        {
            try
            {
                Database.Organizations.Delete(id);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
