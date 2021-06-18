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
using Reestr.BLL.Validation;
using Reestr.DAL.Queries;

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
                List<Organization> organizations = _unitOfWork.Organizations.List(query);
                List<OrganizationDTO> organizationDTOs = Mapper.Map<List<OrganizationDTO>>(organizations);
                return organizationDTOs;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public ValidationResponse Insert(OrganizationDTO organizationDTO)
        {
            var validationResponse = ValidateOrganizationDTO(organizationDTO);
            if (!validationResponse.Status)
                return validationResponse;


            try
            {
                var organization = Mapper.Map<Organization>(organizationDTO);

                _unitOfWork.Organizations.Insert(organization);
            }
            catch (Exception ex)
            {
                validationResponse.Status = false;
                validationResponse.ErrorMessage = ex.Message;
            }

            return validationResponse;
        }

        public ValidationResponse Update(OrganizationDTO organizationDTO)
        {
            var validationResponse = ValidateOrganizationDTO(organizationDTO);
            if (!validationResponse.Status)
                return validationResponse;

            try
            {
                var organization = Mapper.Map<Organization>(organizationDTO);

                _unitOfWork.Organizations.Update(organization);
            }
            catch(Exception ex)
            {
                validationResponse.Status = false;
                validationResponse.ErrorMessage = ex.Message;
            }

            return validationResponse;
        }

        public ValidationResponse Delete(int id)
        {
            var validationResponse = new ValidationResponse();

            try
            {
                _unitOfWork.Organizations.Delete(id);
            }
            catch (Exception ex)
            {
                validationResponse.Status = false;
                validationResponse.ErrorMessage = ex.Message;
            }

            return validationResponse;
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }


        public ValidationResponse ValidateOrganizationDTO(OrganizationDTO model)
        {
            var validationResponse = new ValidationResponse();

            if (model == null)
            {
                validationResponse.ErrorMessage = "Объект не найден.";
                validationResponse.Status = false;
                return validationResponse;
            }


            OrganizationQuery query = new OrganizationQuery() { Id = model.Id, Name = model.Name, IsDeleted = false, Offset = 0, Limit = 10 };
            var organizations = _unitOfWork.Organizations.List(query);
            if (organizations.Any())
            {
                validationResponse.ErrorMessage = "Такое название уже зарегистрировано";
                validationResponse.Status = false;
                return validationResponse;
            }

            if (model.Name.Trim().Length == 0)
            {
                validationResponse.ErrorMessage = "Название организации обязательно к заполнению";
                validationResponse.Status = false;
                return validationResponse;
            }

            if (model.Name.Trim().Length > 300)
            {
                validationResponse.ErrorMessage = "Название организации не должно превышвать 300 символов";
                validationResponse.Status = false;
                return validationResponse;
            }

            query = new OrganizationQuery() { Id = model.Id, BIN = model.BIN.Trim(), IsDeleted = false, Offset = 0, Limit = 10 };
            organizations = _unitOfWork.Organizations.List(query);
            if (organizations.Any())
            {
                validationResponse.ErrorMessage = "Введенный Вами БИН уже зарегистрирован";
                validationResponse.Status = false;
                return validationResponse;
            }

            if (model.BIN.Trim().Length != 12)
            {
                validationResponse.ErrorMessage = "БИН должен содержать ровно 12 символов";
                validationResponse.Status = false;
                return validationResponse;
            }


            if (model.PhoneNumber.Trim().Length != 10)
            {
                validationResponse.ErrorMessage = "Телефон должен включать ровно 10 цифр, без каких-либо других знаков";
                validationResponse.Status = false;
                return validationResponse;
            }

            return validationResponse;
        }
    }
}
