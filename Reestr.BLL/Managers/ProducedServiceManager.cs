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
                var producedService = _unitOfWork.ProducedServices.Get(id);

                return Mapper.Map<ProducedServiceDTO>(producedService);
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
                var producedServices = _unitOfWork.ProducedServices.List(query);

                return Mapper.Map<List<ProducedServiceDTO>>(producedServices);
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
                var producedService = Mapper.Map<ProducedService>(producedServiceDTO);

                _unitOfWork.ProducedServices.Insert(producedService);
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
                var producedService = Mapper.Map<ProducedService>(producedServiceDTO);

                _unitOfWork.ProducedServices.Update(producedService);
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
