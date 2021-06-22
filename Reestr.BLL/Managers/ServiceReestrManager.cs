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
    public class ServiceReestrManager
    {
        private IUnitOfWork _unitOfWork;
        private IMapper Mapper { get; } = AutoMapperConfigurator.GetMapper();
        public ServiceReestrManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public ServiceReestrDTO Get(int id)
        {
            try
            {
                if (id <= 0)
                    throw new Exception("Id cannot be less or equal 0");

                var serviceReestrEntity = _unitOfWork.ServiceReestres.Get(id);

                return Mapper.Map<ServiceReestrDTO>(serviceReestrEntity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<ServiceReestrDTO> List(IQuery query)
        {
            try
            {
                if (query is null)
                    throw new Exception("Query не может быть равен null");

                List<ServiceReestr> serviceReestrEntities = _unitOfWork.ServiceReestres.List(query);

                return Mapper.Map<List<ServiceReestrDTO>>(serviceReestrEntities);
            }
            catch (Exception ex)
            {
                throw ex;
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
            catch (Exception ex)
            {
                validationResponse.Status = false;
                validationResponse.ErrorMessage = ex.Message;
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

                _unitOfWork.ServiceReestres.Delete(id);
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


        public ValidationResponse ValidateServiceReestrDTO(ServiceReestrDTO model)
        {
            var validationResponse = new ValidationResponse();


            if (model == null)
            {
                validationResponse.ErrorMessage = "Объект не найден.";
                validationResponse.Status = false;
                return validationResponse;
            }



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
