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
using log4net;
using Resources;
using Reestr.DAL.Repositories;

namespace Reestr.BLL.Managers
{
    public class ServiceReestrManager
    {
        private IUnitOfWork _unitOfWork;
        private IMapper Mapper { get; } = AutoMapperConfigurator.GetMapper();

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public ServiceReestrManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public ServiceReestrDTO Get(int id)
        {
            try
            {
                if (id <= 0)
                    throw new Exception(Resources_ru.IdLessThanZero);

                var serviceReestrEntity = _unitOfWork.ServiceReestres.Get(id);

                return Mapper.Map<ServiceReestrDTO>(serviceReestrEntity);
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


        public List<ServiceReestrDTO> List(IQuery query)
        {
            try
            {
                var serviceReestrRepository = _unitOfWork.ServiceReestres as ServiceReestrRepository;

                if (query is null)
                    throw new Exception(Resources_ru.ObjectNotFound);

                List<ServiceReestr> serviceReestrEntities = _unitOfWork.ServiceReestres.List(query);

                int totalRecords = serviceReestrRepository.CountRecords(query);

                List<ServiceReestrDTO> serviceReestrDTOs = Mapper.Map<List<ServiceReestrDTO>>(serviceReestrEntities);

                if (serviceReestrDTOs.Any())
                {
                    serviceReestrDTOs.First().TotalRecords = totalRecords;
                }

                return serviceReestrDTOs;
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


        public ValidationResponse Insert(ServiceReestrDTO serviceReestrDTO)
        {
            var validationResponse = ValidateServiceReestrDTO(serviceReestrDTO);
            if (!validationResponse.Status)
                return validationResponse;

            try
            {
                var serviceReestrEntity = Mapper.Map<ServiceReestr>(serviceReestrDTO);

                _unitOfWork.ServiceReestres.Insert(serviceReestrEntity);
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


        public ValidationResponse Update(ServiceReestrDTO serviceReestrDTO)
        {
            var validationResponse = ValidateServiceReestrDTO(serviceReestrDTO);
            if (!validationResponse.Status)
                return validationResponse;

            try
            {
                var serviceReestrEntity = Mapper.Map<ServiceReestr>(serviceReestrDTO);

                _unitOfWork.ServiceReestres.Update(serviceReestrEntity);
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

                _unitOfWork.ServiceReestres.Delete(id);
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


        public ValidationResponse ValidateServiceReestrDTO(ServiceReestrDTO model)
        {
            var validationResponse = new ValidationResponse();


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

            return validationResponse;
        }
    }
}
