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
using System.ComponentModel.DataAnnotations;

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

                var employeeReestrEntity = _unitOfWork.EmployeeReestres.Get(id);

                return Mapper.Map<EmployeeReestrDTO>(employeeReestrEntity);
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
                if (query is null)
                    throw new Exception("Query не может быть равен null");

                var employeeReestrEntities = _unitOfWork.EmployeeReestres.List(query);

                return Mapper.Map<List<EmployeeReestrDTO>>(employeeReestrEntities);
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
                var employeeReestrEntity = Mapper.Map<EmployeeReestr>(employeeReestrDTO);

                _unitOfWork.EmployeeReestres.Insert(employeeReestrEntity);
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
                var employeeReestrEntity = Mapper.Map<EmployeeReestr>(employeeReestrDTO);

                _unitOfWork.EmployeeReestres.Update(employeeReestrEntity);
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
            EmployeeReestrQuery query;
            List<EmployeeReestr> employeeReestrEntities = new List<EmployeeReestr>();


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



            if (model.IIN.Trim().Length != 12)
            {
                validationResponse.ErrorMessage = "ИИН должен содержать ровно 12 цифр";
                validationResponse.Status = false;
                return validationResponse;
            }

            if (model.Id > 0)
            {
                EmployeeReestr employeeReestrEntity = _unitOfWork.EmployeeReestres.Get(model.Id);

                try
                {
                    if (employeeReestrEntity is null)
                        throw new Exception("Объект не найден");

                    if (model.IIN != employeeReestrEntity.IIN)
                    {
                        query = new EmployeeReestrQuery() { Id = model.Id, IIN = model.IIN.Trim(), IsDeleted = false, Offset = 0, Limit = 10 };
                        employeeReestrEntities = _unitOfWork.EmployeeReestres.List(query);
                        if (employeeReestrEntities.Any())
                        {
                            validationResponse.ErrorMessage = "Введенный Вами ИИН уже зарегистрирован";
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
                query = new EmployeeReestrQuery() { Id = model.Id, IIN = model.IIN.Trim(), IsDeleted = false, Offset = 0, Limit = 10 };
                employeeReestrEntities = _unitOfWork.EmployeeReestres.List(query);
                if (employeeReestrEntities.Any())
                {
                    validationResponse.ErrorMessage = "Введенный Вами ИИН уже зарегистрирован";
                    validationResponse.Status = false;
                    return validationResponse;
                }
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

            if (model.Id > 0)
            {
                EmployeeReestr employeeReestrEntity = _unitOfWork.EmployeeReestres.Get(model.Id);

                try
                {
                    if (employeeReestrEntity is null)
                        throw new Exception("Объект не найден");

                    if (model.FullName != employeeReestrEntity.FullName)
                    {
                        query = new EmployeeReestrQuery() { Id = model.Id, FullName = model.FullName, IsDeleted = false, Offset = 0, Limit = 10 };
                        employeeReestrEntities = _unitOfWork.EmployeeReestres.List(query);
                        if (employeeReestrEntities.Any())
                        {
                            validationResponse.ErrorMessage = "Такое ФИО уже зарегистрировано";
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
                query = new EmployeeReestrQuery() { Id = model.Id, FullName = model.FullName, IsDeleted = false, Offset = 0, Limit = 10 };
                employeeReestrEntities = _unitOfWork.EmployeeReestres.List(query);
                if (employeeReestrEntities.Any())
                {
                    validationResponse.ErrorMessage = "Такое ФИО уже зарегистрировано";
                    validationResponse.Status = false;
                    return validationResponse;
                }
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
