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
    public class ProducedServiceManager
    {
        private IUnitOfWork _unitOfWork;
        private IMapper Mapper { get; } = AutoMapperConfigurator.GetMapper();
        public ProducedServiceManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public ProducedServiceDTO Get(int id)
        {
            try
            {
                if (id <= 0)
                    throw new Exception("Id cannot be less or equal 0");

                var producedServiceEntity = _unitOfWork.ProducedServices.Get(id);

                return Mapper.Map<ProducedServiceDTO>(producedServiceEntity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<ProducedServiceDTO> List(IQuery query)
        {
            try
            {
                if (query is null)
                    throw new Exception("Query не может быть равен null");

                var producedServiceEntities = _unitOfWork.ProducedServices.List(query);

                return Mapper.Map<List<ProducedServiceDTO>>(producedServiceEntities);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public ValidationResponse Insert(ProducedServiceDTO producedServiceDTO)
        {
            var validationResponse = ValidateProducedServiceDTO(producedServiceDTO);
            if (!validationResponse.Status)
                return validationResponse;

            try
            {
                var producedServiceEntity = Mapper.Map<ProducedService>(producedServiceDTO);

                _unitOfWork.ProducedServices.Insert(producedServiceEntity);
            }
            catch (Exception ex)
            {
                validationResponse.Status = false;
                validationResponse.ErrorMessage = ex.Message;
            }

            return validationResponse;
        }


        public ValidationResponse Update(ProducedServiceDTO producedServiceDTO)
        {
            var validationResponse = ValidateProducedServiceDTO(producedServiceDTO);
            if (!validationResponse.Status)
                return validationResponse;

            try
            {
                var producedServiceEntity = Mapper.Map<ProducedService>(producedServiceDTO);

                _unitOfWork.ProducedServices.Update(producedServiceEntity);
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

                _unitOfWork.ProducedServices.Delete(id);
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


        public ValidationResponse ValidateProducedServiceDTO(ProducedServiceDTO model)
        {
            var validationResponse = new ValidationResponse();

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
