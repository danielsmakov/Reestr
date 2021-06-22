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
    public class ServiceManager
    {
        private IUnitOfWork _unitOfWork;
        private IMapper Mapper { get; } = AutoMapperConfigurator.GetMapper();
        public ServiceManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public ServiceDTO Get(int id)
        {
            try
            {
                if (id <= 0)
                    throw new Exception("Id cannot be less or equal 0");

                var serviceEntity = _unitOfWork.Services.Get(id);

                return Mapper.Map<ServiceDTO>(serviceEntity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ServiceDTO> List(IQuery query)
        {
            try
            {
                if (query is null)
                    throw new Exception("Query не может быть равен null");

                List<Service> serviceEntities = _unitOfWork.Services.List(query);

                return Mapper.Map<List<ServiceDTO>>(serviceEntities);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ValidationResponse Insert(ServiceDTO serviceDTO)
        {
            var validationResponse = ValidateServiceDTO(serviceDTO);
            if (!validationResponse.Status)
                return validationResponse;

            try
            {
                var serviceEntity = Mapper.Map<Service>(serviceDTO);

                _unitOfWork.Services.Insert(serviceEntity);
            }
            catch (Exception ex)
            {
                validationResponse.Status = false;
                validationResponse.ErrorMessage = ex.Message;
            }

            return validationResponse;
        }

        public ValidationResponse Update(ServiceDTO serviceDTO)
        {
            var validationResponse = ValidateServiceDTO(serviceDTO);
            if (!validationResponse.Status)
                return validationResponse;

            try
            {
                var serviceEntity = Mapper.Map<Service>(serviceDTO);

                _unitOfWork.Services.Update(serviceEntity);
            }
            catch (Exception ex)
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

                _unitOfWork.Services.Delete(id);
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


        public ValidationResponse ValidateServiceDTO(ServiceDTO model)
        {
            var validationResponse = new ValidationResponse();
            ServiceQuery query;
            List<Service> serviceEntities = new List<Service>();

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



            if (model.Name.Trim().Length == 0)
            {
                validationResponse.ErrorMessage = "Название услуги обязательно к заполнению";
                validationResponse.Status = false;
                return validationResponse;
            }

            if (model.Name.Trim().Length > 300)
            {
                validationResponse.ErrorMessage = "Название услуги не должно превышвать 300 символов";
                validationResponse.Status = false;
                return validationResponse;
            }

            if (model.Id > 0)
            {
                Service serviceEntity = _unitOfWork.Services.Get(model.Id);

                try
                {
                    if (serviceEntity is null)
                        throw new Exception("Объект не найден");

                    if (model.Name != serviceEntity.Name)
                    {
                        query = new ServiceQuery() { Id = model.Id, Name = model.Name, IsDeleted = false, Offset = 0, Limit = 10 };
                        serviceEntities.Clear();
                        serviceEntities = _unitOfWork.Services.List(query);
                        if (serviceEntities.Any())
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

            if (model.Id == 0)
            {
                query = new ServiceQuery() { Id = model.Id, Name = model.Name, IsDeleted = false, Offset = 0, Limit = 10 };
                serviceEntities.Clear();
                serviceEntities = _unitOfWork.Services.List(query);
                if (serviceEntities.Any())
                {
                    validationResponse.ErrorMessage = "Такое название уже зарегистрировано";
                    validationResponse.Status = false;
                    return validationResponse;
                }
            }



            if (model.Code.Trim().Length != 9)
            {
                validationResponse.ErrorMessage = "Код должен содержать ровно 9 символов";
                validationResponse.Status = false;
                return validationResponse;
            }

            if (model.Id > 0)
            {
                Service serviceEntity = _unitOfWork.Services.Get(model.Id);

                try
                {
                    if (serviceEntity is null)
                        throw new Exception("Объект не найден");

                    if (model.Code != serviceEntity.Code)
                    {
                        query = new ServiceQuery() { Id = model.Id, Code = model.Code.Trim(), IsDeleted = false, Offset = 0, Limit = 10 };
                        serviceEntities = _unitOfWork.Services.List(query);
                        if (serviceEntities.Any())
                        {
                            validationResponse.ErrorMessage = "Введенный Вами код уже зарегистрирован";
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
                query = new ServiceQuery() { Id = model.Id, Code = model.Code.Trim(), IsDeleted = false, Offset = 0, Limit = 10 };
                serviceEntities = _unitOfWork.Services.List(query);
                if (serviceEntities.Any())
                {
                    validationResponse.ErrorMessage = "Введенный Вами код уже зарегистрирован";
                    validationResponse.Status = false;
                    return validationResponse;
                }
            }



            if (model.Price < 0)
            {
                validationResponse.ErrorMessage = "Цена услуги не может быть отрицательной";
                validationResponse.Status = false;
                return validationResponse;
            }

            return validationResponse;
        }
    }
}
