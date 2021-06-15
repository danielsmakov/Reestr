using Reestr.BLL.DTOs;
using Reestr.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Reestr.DAL.Entities;
using Reestr.BLL.Interfaces;

namespace Reestr.BLL.Managers
{
    public class OrganizationManager
    {
        private IValidationDictionary _validationDictionary;
        private IUnitOfWork _unitOfWork;
        private AutoMapperConfigurator MapperConfiguration { get; } = new AutoMapperConfigurator();
        public OrganizationManager(IValidationDictionary validationDictionary, IUnitOfWork unitOfWork)
        {
            _validationDictionary = validationDictionary;
            _unitOfWork = unitOfWork;
        }
        public OrganizationDTO Get(int id)
        {
            try
            {
                var organization = _unitOfWork.Organizations.Get(id);

                return MapperConfiguration.Mapper.Map<Organization, OrganizationDTO>(organization);
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
                var organizations = _unitOfWork.Organizations.List(query);

                return MapperConfiguration.Mapper.Map<List<Organization>, List<OrganizationDTO>>(organizations);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public bool Insert(OrganizationDTO organizationDTO)
        {
            if (!ValidateOrganizationDTO(organizationDTO))
                return false;

            try
            {
                var organization = MapperConfiguration.Mapper.Map<OrganizationDTO, Organization>(organizationDTO);

                _unitOfWork.Organizations.Insert(organization);
            }
            catch
            {
                return false;
            }

            return true;
        }
        public bool Update(OrganizationDTO organizationDTO)
        {
            if (!ValidateOrganizationDTO(organizationDTO))
                return false;

            try
            {
                var organization = MapperConfiguration.Mapper.Map<OrganizationDTO, Organization>(organizationDTO);

                _unitOfWork.Organizations.Update(organization);
            }
            catch
            {
                return false;
            }

            return true;
        }
        public void Delete(int id)
        {
            try
            {
                _unitOfWork.Organizations.Delete(id);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public void Dispose()
        {
            _unitOfWork.Dispose();
        }


        protected bool ValidateOrganizationDTO(OrganizationDTO organizationDTO)
        {
            if (organizationDTO.Name.Trim().Length == 0)
                _validationDictionary.AddError("Name", "Name is required.");
            if (organizationDTO.BIN.Trim().Length != 12)
                _validationDictionary.AddError("BIN", "BIN must be exactly 12 digits long.");
            if (organizationDTO.PhoneNumber.Trim().Length != 10)
                _validationDictionary.AddError("PhoneNumber", "You should enter 10 digits only in the Phone Number field.");
            return _validationDictionary.IsValid;
        }
    }
}
