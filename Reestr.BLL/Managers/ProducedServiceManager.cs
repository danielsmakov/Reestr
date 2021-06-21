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
            var validationResponse = new ValidationResponse();
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
            var validationResponse = new ValidationResponse();
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


        /*public ValidationResponse ValidateProducedServiceDTO(ProducedServiceDTO producedServiceDTO)
        {
            var validationResponse = new ValidationResponse();





            return validationResponse;
        }*/
    }
}
