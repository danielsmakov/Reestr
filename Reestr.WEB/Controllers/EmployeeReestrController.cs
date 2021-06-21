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

            return Json(employeeReestrDTOs, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Insert(EmployeeReestrDTO employeeReestrDTO)
        {
            if (ModelState.IsValid)
            {
                var validationResponse = _employeeReestrManager.Insert(employeeReestrDTO);
                // TODO: отправить ошибки из validationResponse в UI
            }


            // TODO: узнать, в каком виде отправлять ответ из метода Insert
            return Json(new[] { employeeReestrDTO });
        }


        public ActionResult Update(EmployeeReestrDTO employeeReestrDTO)
        {
            if (ModelState.IsValid)
            {
                var validationResponse = _employeeReestrManager.Update(employeeReestrDTO);
                // TODO: отправить ошибки из validationResponse в UI
            }

            // TODO: узнать, в каком виде отправлять ответ из метода Update
            return Json(new[] { employeeReestrDTO });
        }


        public ActionResult Delete(EmployeeReestrDTO employeeReestrDTO)
        {
            var validationResponse = _employeeReestrManager.Delete(employeeReestrDTO.Id);
            // TODO: отправить ошибки из validationResponse в UI

            // TODO: узнать, в каком виде отправлять ответ из метода Delete
            return Json(new[] { employeeReestrDTO });
        }
    }
}