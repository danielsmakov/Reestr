using Reestr.BLL.DTOs;
using Reestr.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Reestr.DAL.Entities;
using Reestr.DAL.Queries;
using Reestr.BLL.Validation;

namespace Reestr.BLL.Managers
{
    public class EmployeeReestrManager
    {
        private IUnitOfWork _unitOfWork;
        private IMapper Mapper { get; } = AutoMapperConfigurator.GetMapper();
        public EmployeeReestrManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public EmployeeReestrDTO Get(int id)
        {
            try
            {
                if (id <= 0)
                    throw new Exception("Id cannot be less or equal 0");

                var employeeReestr = _unitOfWork.EmployeeReestres.Get(id);

                return Mapper.Map<EmployeeReestrDTO>(employeeReestr);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<EmployeeReestrDTO> List(IQuery query)
        {
            try
            {
                var employeeReestres = _unitOfWork.EmployeeReestres.List(query);

                return Mapper.Map<List<EmployeeReestrDTO>>(employeeReestres);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public ValidationResponse Insert(EmployeeReestrDTO employeeReestrDTO)
        {
            var validationResponse = ValidateEmployeeReestrDTO(employeeReestrDTO);
            if (!validationResponse.Status)
                return validationResponse;

            try
            {
                var employeeReestr = Mapper.Map<EmployeeReestr>(employeeReestrDTO);

                _unitOfWork.EmployeeReestres.Insert(employeeReestr);
            }
            catch (Exception ex)
            {
                validationResponse.Status = false;
                validationResponse.ErrorMessage = ex.Message;
            }

            return validationResponse;
        }


        public ValidationResponse Update(EmployeeReestrDTO employeeReestrDTO)
        {
            var validationResponse = ValidateEmployeeReestrDTO(employeeReestrDTO);
            if (!validationResponse.Status)
                return validationResponse;

            try
            {
                var employeeReestr = Mapper.Map<EmployeeReestr>(employeeReestrDTO);

                _unitOfWork.EmployeeReestres.Update(employeeReestr);
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

                _unitOfWork.EmployeeReestres.Delete(id);
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


        public ValidationResponse ValidateEmployeeReestrDTO(EmployeeReestrDTO model)
        {
            var validationResponse = new ValidationResponse();


            if (model == null)
            {
                validationResponse.ErrorMessage = "Объект не найден.";
                validationResponse.Status = false;
                return validationResponse;
            }


            EmployeeReestrQuery query = new EmployeeReestrQuery() { Id = model.Id, IIN = model.IIN.Trim(), IsDeleted = false, Offset = 0, Limit = 10 };
            List<EmployeeReestr> employeeReestres = _unitOfWork.EmployeeReestres.List(query);
            if (employeeReestres.Any())
            {
                validationResponse.ErrorMessage = "Введенный Вами ИИН уже зарегистрирован";
                validationResponse.Status = false;
                return validationResponse;
            }

            if (model.IIN.Trim().Length != 12)
            {
                validationResponse.ErrorMessage = "ИИН должен содержать ровно 12 символов";
                validationResponse.Status = false;
                return validationResponse;
            }


            query = new EmployeeReestrQuery() { Id = model.Id, FullName = model.FullName, IsDeleted = false, Offset = 0, Limit = 10 };
            employeeReestres = _unitOfWork.EmployeeReestres.List(query);
            if (employeeReestres.Any())
            {
                validationResponse.ErrorMessage = "Такое ФИО уже зарегистрировано";
                validationResponse.Status = false;
                return validationResponse;
            }

            if (model.FullName.Trim().Length == 0)
            {
                validationResponse.ErrorMessage = "ФИО обязательно к заполнению";
                validationResponse.Status = false;
                return validationResponse;
            }

            if (model.FullName.Trim().Length > 150)
            {
                validationResponse.ErrorMessage = "ФИО не должно быть длиннее 150 символов";
                validationResponse.Status = false;
                return validationResponse;
            }


            if (model.DateOfBirth > DateTime.Now)
            {
                validationResponse.ErrorMessage = "Дата рождения не может относиться к будущему";
                validationResponse.Status = false;
                return validationResponse;
            }

            DateTime dateTime = new DateTime(1899, 1, 1, 0, 0, 0);
            if (model.DateOfBirth < dateTime)
            {
                validationResponse.ErrorMessage = "Дата рождения не может быть раньше 1899 года";
                validationResponse.Status = false;
                return validationResponse;
            }


            if (model.PhoneNumber.Trim().Length != 10)
            {
                validationResponse.ErrorMessage = "Телефон должен включать ровно 10 цифр, без каких-либо других знаков";
                validationResponse.Status = false;
                return validationResponse;
            }


            if (model.BeginDate < model.DateOfBirth)
            {
                validationResponse.ErrorMessage = "Дата найма не может быть раньше даты рождения";
                validationResponse.Status = false;
                return validationResponse;
            }

            return validationResponse;
        }
    }
}
