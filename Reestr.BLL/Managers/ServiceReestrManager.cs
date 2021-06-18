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
                var serviceReestr = _unitOfWork.ServiceReestres.Get(id);

                return Mapper.Map<ServiceReestrDTO>(serviceReestr);
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
                List<ServiceReestr> serviceReestres = _unitOfWork.ServiceReestres.List(query);

                return Mapper.Map<List<ServiceReestrDTO>>(serviceReestres);
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
                var serviceReestr = Mapper.Map<ServiceReestr>(serviceReestrDTO);

                _unitOfWork.ServiceReestres.Insert(serviceReestr);
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
                var serviceReestr = Mapper.Map<ServiceReestr>(serviceReestrDTO);

                _unitOfWork.ServiceReestres.Update(serviceReestr);
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
