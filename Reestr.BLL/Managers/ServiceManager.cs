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
using log4net;

namespace Reestr.BLL.Managers
{
    public class ServiceManager
    {
        private IUnitOfWork _unitOfWork;
        private IMapper Mapper { get; } = AutoMapperConfigurator.GetMapper();
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public ServiceManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public ServiceDTO Get(int id)
        {
            try
            {
                if (id <= 0)
                    throw new Exception(Resources_ru.IdLessThanZero);

                var serviceEntity = _unitOfWork.Services.Get(id);

                ServiceDTO serviceDTO = Mapper.Map<ServiceDTO>(serviceEntity);

                return serviceDTO;
            }
            catch (ApplicationException)
            {
                throw new ApplicationException(Resources_ru.ErrorInRepositories);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new ApplicationException(Resources_ru.ErrorInRepositories);
            }
        }

        public List<ServiceDTO> List(IQuery query)
        {
            var serviceRepository = _unitOfWork.Services as ServiceRepository;
            try
            {
                if (query is null)
                    throw new Exception(Resources_ru.ObjectNotFound);

                List<Service> serviceEntities = _unitOfWork.Services.List(query);

                int totalRecords = serviceRepository.CountRecords(query);

                List<ServiceDTO> serviceDTOs = Mapper.Map<List<ServiceDTO>>(serviceEntities);

                if (serviceDTOs.Any())
                {
                    serviceDTOs.First().TotalRecords = totalRecords;
                }

                return serviceDTOs;
            }
            catch (ApplicationException)
            {
                throw new ApplicationException(Resources_ru.ErrorInRepositories);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new ApplicationException(Resources_ru.ErrorInRepositories);
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
            catch (ApplicationException)
            {
                throw new ApplicationException(Resources_ru.ErrorInRepositories);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new ApplicationException(Resources_ru.ErrorInRepositories);
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
            catch (ApplicationException)
            {
                throw new ApplicationException(Resources_ru.ErrorInRepositories);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new ApplicationException(Resources_ru.ErrorInRepositories);
            }

            return validationResponse;
        }

        public ValidationResponse Delete(int id)
        {
            var validationResponse = new ValidationResponse();

            try
            {
                if (id <= 0)
                    throw new Exception(Resources_ru.IdLessThanZero);

                _unitOfWork.Services.Delete(id);
            }
            catch (ApplicationException)
            {
                throw new ApplicationException(Resources_ru.ErrorInRepositories);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new ApplicationException(Resources_ru.ErrorInRepositories);
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
                validationResponse.ErrorMessage = Resources_ru.ObjectNotFound;
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


            if (model.Id > 0)
            {
                Service serviceEntity = _unitOfWork.Services.Get(model.Id);

                try
                {
                    if (serviceEntity is null)
                        throw new Exception(Resources_ru.ObjectNotFound);

                    if (model.Name != serviceEntity.Name)
                    {
                        query = new ServiceQuery() { Id = model.Id, Name = model.Name, IsDeleted = false, Offset = 0, Limit = 10 };
                        serviceEntities.Clear();
                        serviceEntities = _unitOfWork.Services.List(query);
                        if (serviceEntities.Any())
                        {
                            validationResponse.ErrorMessage = Resources_ru.DataUniqueness;
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
                    validationResponse.ErrorMessage = Resources_ru.DataUniqueness;
                    validationResponse.Status = false;
                    return validationResponse;
                }
            }


            if (model.Id > 0)
            {
                Service serviceEntity = _unitOfWork.Services.Get(model.Id);

                try
                {
                    if (serviceEntity is null)
                        throw new Exception(Resources_ru.ObjectNotFound);

                    if (model.Code != serviceEntity.Code)
                    {
                        query = new ServiceQuery() { Id = model.Id, Code = model.Code.Trim(), IsDeleted = false, Offset = 0, Limit = 10 };
                        serviceEntities.Clear();
                        serviceEntities = _unitOfWork.Services.List(query);
                        if (serviceEntities.Any())
                        {
                            validationResponse.ErrorMessage = Resources_ru.DataUniqueness;
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
                serviceEntities.Clear();
                serviceEntities = _unitOfWork.Services.List(query);
                if (serviceEntities.Any())
                {
                    validationResponse.ErrorMessage = Resources_ru.DataUniqueness;
                    validationResponse.Status = false;
                    return validationResponse;
                }
            }


            return validationResponse;
        }
    }
}
