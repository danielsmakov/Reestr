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
    public class ServiceReestrController : Controller
    {
        private ServiceReestrManager _serviceReestrManager;
        public ServiceReestrController()
        {
            _serviceReestrManager = new ServiceReestrManager(new UnitOfWork());
        }


        public ServiceReestrController(ServiceReestrManager serviceReestrManager)
        {
            _serviceReestrManager = serviceReestrManager;
        }


        public ActionResult Index()
        {
            return View("ServiceReestr");
        }


        public ActionResult List()
        {
            ServiceReestrQuery query = new ServiceReestrQuery();
            query.Offset = 0;
            query.Limit = 20;
            List<ServiceReestrDTO> serviceReestrDTOs = _serviceReestrManager.List(query);

            return Json(serviceReestrDTOs);
        }

        [HttpPost]
        public ActionResult Insert(ServiceReestrDTO serviceReestrDTO)
        {
            var validationResponse = _serviceReestrManager.Insert(serviceReestrDTO);
            
            return Json(validationResponse);
        }

        [HttpPost]
        public ActionResult Update(ServiceReestrDTO serviceReestrDTO)
        {
            var validationResponse = _serviceReestrManager.Update(serviceReestrDTO);

            return Json(validationResponse);
        }

        [HttpPost]
        public ActionResult Delete(ServiceReestrDTO serviceReestrDTO)
        {
            var validationResponse = _serviceReestrManager.Delete(serviceReestrDTO.Id);
            
            return Json(validationResponse);
        }
    }
}