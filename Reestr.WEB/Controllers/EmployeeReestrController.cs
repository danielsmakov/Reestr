using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Reestr.BLL.DTOs;
using Reestr.BLL.Managers;
using Reestr.BLL.Validation;
using Reestr.DAL.Queries;
using Reestr.DAL.Repositories;
using System.Web.ModelBinding;

namespace Reestr.WEB.Controllers
{
    public class EmployeeReestrController : Controller
    {
        private EmployeeReestrManager _employeeReestrManager;
        public EmployeeReestrController()
        {
            _employeeReestrManager = new EmployeeReestrManager(new UnitOfWork());
        }


        public EmployeeReestrController(EmployeeReestrManager employeeReestrManager)
        {
            _employeeReestrManager = employeeReestrManager;
        }


        public ActionResult Index()
        {
            return View("EmployeeReestr");
        }


        public ActionResult List()
        {
            EmployeeReestrQuery query = new EmployeeReestrQuery();
            query.Offset = 0;
            query.Limit = 20;
            List<EmployeeReestrDTO> employeeReestrDTOs = _employeeReestrManager.List(query);

            return Json(employeeReestrDTOs);
        }

        [HttpPost]
        public ActionResult Insert(EmployeeReestrDTO employeeReestrDTO)
        {
            var validationResponse = _employeeReestrManager.Insert(employeeReestrDTO);
            
            return Json(validationResponse);
        }

        [HttpPost]
        public ActionResult Update(EmployeeReestrDTO employeeReestrDTO)
        {
            var validationResponse = _employeeReestrManager.Update(employeeReestrDTO);

            return Json(validationResponse);
        }

        [HttpPost]
        public ActionResult Delete(EmployeeReestrDTO employeeReestrDTO)
        {
            var validationResponse = _employeeReestrManager.Delete(employeeReestrDTO.Id);
            
            return Json(validationResponse);
        }
    }
}