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
using Reestr.DAL.Repositories;
using Resources;

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
                    throw new Exception("Id не может быть равен или меньше 0");

                var organizationEntity = _unitOfWork.Organizations.Get(id);

                return Mapper.Map<OrganizationDTO>(organizationEntity);
            }
            catch(Exception)
            {
                throw new Exception(Resources_ru.ErrorInRepositories);
            }
        }

        public List<OrganizationDTO> List(IQuery query)
        {
            var organizationRepository = _unitOfWork.Organizations as OrganizationRepository;
            try
            {
                if (query is null)
                    throw new Exception("Query не может быть равен null");

                List<Organization> organizationEntities = _unitOfWork.Organizations.List(query);

                int totalRecords = organizationRepository.CountRecords(query);

                List<OrganizationDTO> organizationDTOs = Mapper.Map<List<OrganizationDTO>>(organizationEntities);

                if (organizationDTOs.Any())
                {
                    organizationDTOs.First().TotalRecords = totalRecords;
                }

                return organizationDTOs;
            }
            catch(Exception)
            {
                throw new Exception(Resources_ru.ErrorInRepositories);
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
            catch (Exception)
            {
                validationResponse.Status = false;
                validationResponse.ErrorMessage = Resources_ru.ErrorInRepositories;
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
            catch(Exception)
            {
                validationResponse.Status = false;
                validationResponse.ErrorMessage = Resources_ru.ErrorInRepositories;
            }

            return validationResponse;
        }

        public ValidationResponse Delete(int id)
        {
            var validationResponse = new ValidationResponse();

            try
            {
                if (id <= 0)
                    throw new Exception("Id не может быть равен или меньше 0");
                _unitOfWork.Organizations.Delete(id);
            }
            catch (Exception)
            {
                validationResponse.Status = false;
                validationResponse.ErrorMessage = Resources_ru.ErrorInRepositories;
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

            return validationResponse;
        }
    }
}
