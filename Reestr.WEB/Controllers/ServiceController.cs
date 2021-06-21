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
    public class ServiceController : Controller
    {
        private ServiceManager _serviceManager;
        public ServiceController()
        {
            _serviceManager = new ServiceManager(new UnitOfWork());
        }


        public ServiceController(ServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }


        public ActionResult Index()
        {
            return View("Services");
        }


        public ActionResult List()
        {
            ServiceQuery query = new ServiceQuery();
            query.Offset = 0;
            query.Limit = 20;
            List<ServiceDTO> serviceDTOs = _serviceManager.List(query);

            return Json(serviceDTOs);
        }

        [HttpPost]
        public ActionResult Insert(ServiceDTO serviceDTO)
        {
            var validationResponse = _serviceManager.Insert(serviceDTO);

            return Json(validationResponse);
        }

        [HttpPost]
        public ActionResult Update(ServiceDTO serviceDTO)
        {
            var validationResponse = _serviceManager.Update(serviceDTO);
            
            return Json(validationResponse);
        }

        [HttpPost]
        public ActionResult Delete(ServiceDTO serviceDTO)
        {
            var validationResponse = _serviceManager.Delete(serviceDTO.Id);
            
            return Json(validationResponse);
        }
    }
}