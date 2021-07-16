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
using log4net;
using Resources;
using Reestr.DAL.Repositories;

namespace Reestr.BLL.Managers
{
    public class EmployeeReestrManager
    {
        private IUnitOfWork _unitOfWork;
        private IMapper Mapper { get; } = AutoMapperConfigurator.GetMapper();

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public EmployeeReestrManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public EmployeeReestrDTO Get(int id)
        {
            try
            {
                if (id <= 0)
                    throw new Exception(Resources_ru.IdLessThanZero);

                var employeeReestrEntity = _unitOfWork.EmployeeReestres.Get(id);

                return Mapper.Map<EmployeeReestrDTO>(employeeReestrEntity);
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


        public List<EmployeeReestrDTO> List(IQuery query)
        {
            try
            {
                var employeeReestrRepository = _unitOfWork.EmployeeReestres as EmployeeReestrRepository;

                if (query is null)
                    throw new Exception(Resources_ru.ObjectNotFound);

                List<EmployeeReestr> employeeReestrEntities = _unitOfWork.EmployeeReestres.List(query);

                int totalRecords = employeeReestrRepository.CountRecords(query);

                List<EmployeeReestrDTO> employeeReestrDTOs = Mapper.Map<List<EmployeeReestrDTO>>(employeeReestrEntities);

                if (employeeReestrDTOs.Any())
                {
                    employeeReestrDTOs.First().TotalRecords = totalRecords;
                }

                return employeeReestrDTOs;
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


        public ValidationResponse Insert(EmployeeReestrDTO employeeReestrDTO)
        {
            if (employeeReestrDTO.PhoneNumber is null || employeeReestrDTO.IIN is null)
            {
                throw new Exception(Resources_ru.ObjectNotFound);
            }

            employeeReestrDTO.IIN = MaskCharactersHandler.RemoveEveryCharacterExceptForDigits(employeeReestrDTO.IIN);
            employeeReestrDTO.PhoneNumber = MaskCharactersHandler.RemoveEveryCharacterExceptForDigits(employeeReestrDTO.PhoneNumber);

            var validationResponse = ValidateEmployeeReestrDTO(employeeReestrDTO);
            if (!validationResponse.Status)
                return validationResponse;

            try
            {
                var employeeReestrEntity = Mapper.Map<EmployeeReestr>(employeeReestrDTO);

                _unitOfWork.EmployeeReestres.Insert(employeeReestrEntity);
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


        public ValidationResponse Update(EmployeeReestrDTO employeeReestrDTO)
        {
            if (employeeReestrDTO.PhoneNumber is null || employeeReestrDTO.IIN is null)
            {
                throw new Exception(Resources_ru.ObjectNotFound);
            }

            employeeReestrDTO.IIN = MaskCharactersHandler.RemoveEveryCharacterExceptForDigits(employeeReestrDTO.IIN);
            employeeReestrDTO.PhoneNumber = MaskCharactersHandler.RemoveEveryCharacterExceptForDigits(employeeReestrDTO.PhoneNumber);

            var validationResponse = ValidateEmployeeReestrDTO(employeeReestrDTO);
            if (!validationResponse.Status)
                return validationResponse;

            try
            {
                var employeeReestrEntity = Mapper.Map<EmployeeReestr>(employeeReestrDTO);

                _unitOfWork.EmployeeReestres.Update(employeeReestrEntity);
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

                _unitOfWork.EmployeeReestres.Delete(id);
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


        public ValidationResponse ValidateEmployeeReestrDTO(EmployeeReestrDTO model)
        {
            var validationResponse = new ValidationResponse();
            EmployeeReestrQuery query;
            List<EmployeeReestr> employeeReestrEntities = new List<EmployeeReestr>();


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
                EmployeeReestr employeeReestrEntity = _unitOfWork.EmployeeReestres.Get(model.Id);

                try
                {
                    if (employeeReestrEntity is null)
                        throw new Exception(Resources_ru.ObjectNotFound);

                    if (model.IIN != employeeReestrEntity.IIN)
                    {
                        query = new EmployeeReestrQuery() { Id = model.Id, IIN = model.IIN.Trim(), IsDeleted = false, Offset = 0, Limit = 10 };
                        employeeReestrEntities.Clear();
                        employeeReestrEntities = _unitOfWork.EmployeeReestres.List(query);
                        if (employeeReestrEntities.Any())
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
                query = new EmployeeReestrQuery() { Id = model.Id, IIN = model.IIN.Trim(), IsDeleted = false, Offset = 0, Limit = 10 };
                employeeReestrEntities.Clear();
                employeeReestrEntities = _unitOfWork.EmployeeReestres.List(query);
                if (employeeReestrEntities.Any())
                {
                    validationResponse.ErrorMessage = Resources_ru.DataUniqueness;
                    validationResponse.Status = false;
                    return validationResponse;
                }
            }




            if (model.Id > 0)
            {
                EmployeeReestr employeeReestrEntity = _unitOfWork.EmployeeReestres.Get(model.Id);

                try
                {
                    if (employeeReestrEntity is null)
                        throw new Exception(Resources_ru.ObjectNotFound);

                    if (model.FullName != employeeReestrEntity.FullName)
                    {
                        query = new EmployeeReestrQuery() { Id = model.Id, FullName = model.FullName, IsDeleted = false, Offset = 0, Limit = 10 };
                        employeeReestrEntities.Clear();
                        employeeReestrEntities = _unitOfWork.EmployeeReestres.List(query);
                        if (employeeReestrEntities.Any())
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
                query = new EmployeeReestrQuery() { Id = model.Id, FullName = model.FullName, IsDeleted = false, Offset = 0, Limit = 10 };
                employeeReestrEntities.Clear();
                employeeReestrEntities = _unitOfWork.EmployeeReestres.List(query);
                if (employeeReestrEntities.Any())
                {
                    validationResponse.ErrorMessage = Resources_ru.DataUniqueness;
                    validationResponse.Status = false;
                    return validationResponse;
                }
            }
            


            if (model.DateOfBirth > DateTime.Now)
            {
                validationResponse.ErrorMessage = Resources_ru.DateOfBirthInFuture;
                validationResponse.Status = false;
                return validationResponse;
            }

            DateTime dateTime = new DateTime(1899, 1, 1, 0, 0, 0);
            if (model.DateOfBirth < dateTime)
            {
                validationResponse.ErrorMessage = Resources_ru.DateOfBirthTooEarly;
                validationResponse.Status = false;
                return validationResponse;
            }




            if (model.BeginDate < model.DateOfBirth)
            {
                validationResponse.ErrorMessage = Resources_ru.DateOfHiringEarlierThanDateOfBirth;
                validationResponse.Status = false;
                return validationResponse;
            }

            return validationResponse;
        }
    }
}
