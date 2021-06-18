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
                List<OrganizationDTO> organizationDTOs = Mapper.Map<List<Organization>, List<OrganizationDTO>>(organizations);
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
                validationResponse.ErrorMessages.Add(ex.Message);
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
                validationResponse.ErrorMessages.Add(ex.Message);
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
                validationResponse.ErrorMessages.Add(ex.Message);
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
                validationResponse.ErrorMessages.Add("Объект не найден.");
                validationResponse.Status = false;
                return validationResponse;
            }


            if (model.Name.Trim().Length == 0)
            {
                validationResponse.ErrorMessages.Add("Имя обязательно к заполнению.");
                validationResponse.Status = false;
            }

            if (model.Name.Trim().Length > 300)
            {
                validationResponse.ErrorMessages.Add("Длина имена не должна превышвать 300 символов.");
                validationResponse.Status = false;
            }


            if (model.BIN.Trim().Length != 12)
            {
                validationResponse.ErrorMessages.Add("БИН должен содержать ровно 12 символов.");
                validationResponse.Status = false;
            }

            OrganizationQuery query = new OrganizationQuery() { BIN = model.BIN.Trim(), Offset = 0, Limit = 20 };
            var organizations = _unitOfWork.Organizations.List(query);
            if (organizations.Any())
            {
                validationResponse.ErrorMessages.Add("Введенный Вами БИН уже зарегистрирован.");
                validationResponse.Status = false;
            }


            if (model.PhoneNumber.Trim().Length != 10)
            {
                validationResponse.ErrorMessages.Add("Телефон должен включать только 10 цифр, без какихлибо других знаков.");
                validationResponse.Status = false;
            }

            return validationResponse;
        }
    }
}
