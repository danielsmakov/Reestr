using Reestr.BLL.DTOs;
using Reestr.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Reestr.DAL.Entities;
using Reestr.BLL.Interfaces;

namespace Reestr.BLL.Managers
{
    class ServiceManager
    {
        private IValidationDictionary _validationDictionary;
        private IUnitOfWork _unitOfWork;
        private IMapper Mapper { get; } = AutoMapperConfigurator.GetMapper();
        public ServiceManager(IValidationDictionary validationDictionary, IUnitOfWork unitOfWork)
        {
            _validationDictionary = validationDictionary;
            _unitOfWork = unitOfWork;
        }

        public ServiceDTO Get(int id)
        {
            try
            {
                var service = _unitOfWork.Services.Get(id);

                return Mapper.Map<Service, ServiceDTO>(service);
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
                var services = _unitOfWork.Services.List(query);

                return Mapper.Map<List<Service>, List<ServiceDTO>>(services);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Insert(ServiceDTO serviceDTO)
        {
            if (!ValidateServiceDTO(serviceDTO))
                return false;

            try
            {
                var service = Mapper.Map<ServiceDTO, Service>(serviceDTO);

                _unitOfWork.Services.Insert(service);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public bool Update(ServiceDTO serviceDTO)
        {
            if (!ValidateServiceDTO(serviceDTO))
                return false;

            try
            {
                var service = Mapper.Map<ServiceDTO, Service>(serviceDTO);

                _unitOfWork.Services.Update(service);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public bool Delete(int id)
        {
            try
            {
                _unitOfWork.Services.Delete(id);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }


        protected bool ValidateServiceDTO(ServiceDTO serviceDTO)
        {
            if (serviceDTO.Name.Trim().Length == 0)
                _validationDictionary.AddError("Name", "Name is required.");
            if (serviceDTO.Name.Trim().Length > 300)
                _validationDictionary.AddError("Name", "Name must be less than 300 symbols long.");
            if (serviceDTO.BIN.Trim().Length != 12)
                _validationDictionary.AddError("BIN", "BIN must be exactly 12 digits long.");
            if (serviceDTO.PhoneNumber.Trim().Length != 10)
                _validationDictionary.AddError("PhoneNumber", "You should enter 10 digits only in the Phone Number field.");
            return _validationDictionary.IsValid;
        }
    }
}
