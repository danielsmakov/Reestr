using Reestr.BLL.DTOs;
using Reestr.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Reestr.DAL.Entities;
using Reestr.BLL.Validation;
using Reestr.DAL.Queries;
using System.ComponentModel.DataAnnotations;

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
                if (id <= 0)
                    throw new Exception("Id cannot be less or equal 0");
                var organizationEntity = _unitOfWork.Organizations.Get(id);

                return Mapper.Map<OrganizationDTO>(organizationEntity);
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
                if (query is null)
                    throw new Exception("Query не может быть равен null");

                List<Organization> organizationEntities = _unitOfWork.Organizations.List(query);

                return Mapper.Map<List<OrganizationDTO>>(organizationEntities);
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
                var organizationEntity = Mapper.Map<Organization>(organizationDTO);

                _unitOfWork.Organizations.Insert(organizationEntity);
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
                var organizationEntity = Mapper.Map<Organization>(organizationDTO);

                _unitOfWork.Organizations.Update(organizationEntity);
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
                if (id <= 0)
                    throw new Exception("Id cannot be less or equal 0");
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
            OrganizationQuery query;
            List<Organization> organizationEntities = new List<Organization>();

            if (model == null)
            {
                validationResponse.ErrorMessage = "Объект не найден.";
                validationResponse.Status = false;
                return validationResponse;
            }


            // Валидация на основе атрибутов модели
            var results = new List<ValidationResult>();
            var context = new System.ComponentModel.DataAnnotations.ValidationContext(model);
            if (!Validator.TryValidateObject(model, context, results, true))
            {
                foreach (var error in results)
                {
                    validationResponse.Status = false;
                    validationResponse.ErrorMessage = error.ErrorMessage;
                    return validationResponse;
                }
            }

            
            // Проверка названия организации на кол-во символов
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

            
            if (model.Id > 0) // Выявление операции Update() - Id больше дефолтного значения
            {
                Organization organizationEntity = _unitOfWork.Organizations.Get(model.Id);
                try
                {
                    if (organizationEntity is null)
                        throw new Exception("Объект не найден.");

                    if (model.Name != organizationEntity.Name) // Проверяю, изменил ли пользователь название организации
                    {
                        // Ниже делается запрос в базу на предмет уникальности названия организации
                        query = new OrganizationQuery() { Id = model.Id, Name = model.Name, IsDeleted = false, Offset = 0, Limit = 10 };
                        organizationEntities.Clear();
                        organizationEntities = _unitOfWork.Organizations.List(query);

                        // если из базы пришла хоть одна запись, значит название не уникально
                        if (organizationEntities.Any()) 
                        {
                            validationResponse.ErrorMessage = "Такое название уже зарегистрировано";
                            validationResponse.Status = false;
                            return validationResponse;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            if (model.Id == 0) // Выявление операции Insert() - Id равен дефолтному значению
            {
                query = new OrganizationQuery() { Id = model.Id, Name = model.Name, IsDeleted = false, Offset = 0, Limit = 10 };
                organizationEntities.Clear();
                organizationEntities = _unitOfWork.Organizations.List(query);
                if (organizationEntities.Any())
                {
                    validationResponse.ErrorMessage = "Такое название уже зарегистрировано";
                    validationResponse.Status = false;
                    return validationResponse;
                }
            }


            
            if (model.BIN.Trim().Length != 12)
            {
                validationResponse.ErrorMessage = "БИН должен содержать ровно 12 символов";
                validationResponse.Status = false;
                return validationResponse;
            }

            // Уникальность БИНа проверяется аналогично уникальности названия организации
            if (model.Id > 0)
            {
                Organization organizationEntity = _unitOfWork.Organizations.Get(model.Id);
                try
                {
                    if (organizationEntity is null)
                        throw new Exception("Объект не найден.");

                    if (model.BIN != organizationEntity.BIN)
                    {
                        query = new OrganizationQuery() { Id = model.Id, BIN = model.BIN.Trim(), IsDeleted = false, Offset = 0, Limit = 10 };
                        organizationEntities.Clear();
                        organizationEntities = _unitOfWork.Organizations.List(query);
                        if (organizationEntities.Any())
                        {
                            validationResponse.ErrorMessage = "Введенный Вами БИН уже зарегистрирован";
                            validationResponse.Status = false;
                            return validationResponse;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            if (model.Id == 0)
            {
                query = new OrganizationQuery() { Id = model.Id, BIN = model.BIN.Trim(), IsDeleted = false, Offset = 0, Limit = 10 };
                organizationEntities.Clear();
                organizationEntities = _unitOfWork.Organizations.List(query);
                if (organizationEntities.Any())
                {
                    validationResponse.ErrorMessage = "Введенный Вами БИН уже зарегистрирован";
                    validationResponse.Status = false;
                    return validationResponse;
                }
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
