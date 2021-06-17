﻿using Reestr.BLL.DTOs;
using Reestr.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Reestr.DAL.Entities;
using Reestr.BLL.Interfaces;
using Reestr.BLL.Validation;

namespace Reestr.BLL.Managers
{
    public class OrganizationManager
    {
        private IUnitOfWork _unitOfWork;
        private IMapper Mapper { get; } = AutoMapperConfigurator.GetMapper();
        public OrganizationManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public OrganizationDTO Get(int id)
        {
            try
            {
                var organization = _unitOfWork.Organizations.Get(id);

                return Mapper.Map<OrganizationDTO>(organization);
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

                return Mapper.Map<List<OrganizationDTO>>(organizations);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public CustomResponce Insert(OrganizationDTO organizationDTO)
        {
            var isValid = Validate(organizationDTO);
            if (!isValid.Status)
                return isValid;


            try
            {
                var organization = Mapper.Map<Organization>(organizationDTO);

                _unitOfWork.Organizations.Insert(organization);
            }
            catch (Exception ex)
            {
                isValid.Status = true;
                isValid.Message = ex.Message;
            }

            return isValid;
        }

        public bool Update(OrganizationDTO organizationDTO)
        {
            if (!ValidateOrganizationDTO(organizationDTO))
                return false;

            try
            {
                var organization = Mapper.Map<Organization>(organizationDTO);

                _unitOfWork.Organizations.Update(organization);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public bool Delete(int id)
        {
            try
            {
                _unitOfWork.Organizations.Delete(id);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }


        protected bool ValidateOrganizationDTO(OrganizationDTO organizationDTO)
        {
            if (organizationDTO.Name.Trim().Length == 0)
                _validationDictionary.AddError("Name", "Name is required.");
            if (organizationDTO.Name.Trim().Length > 300)
                _validationDictionary.AddError("Name", "Name must be less than 300 symbols long.");
            if (organizationDTO.BIN.Trim().Length != 12)
                _validationDictionary.AddError("BIN", "BIN must be exactly 12 digits long.");
            if (organizationDTO.PhoneNumber.Trim().Length != 10)
                _validationDictionary.AddError("PhoneNumber", "You should enter 10 digits only in the Phone Number field.");
            return _validationDictionary.IsValid;
        }

        public ValidationResponse Validate(OrganizationDTO model)
        {
            var response = new ValidationResponse();

            if (model == null)
            {
                response.Status = true;
                response.Message = "";
            }


            // ............


            return response;               
        }

        
    }

    public class ValidationResponse
    {
        public bool Status { get; set; }
        public string Message { get; set; }
    }
}
