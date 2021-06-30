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
using Reestr.WEB.Models;

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


        public ActionResult List(ServiceQuery query)
        {

            List<ServiceDTO> serviceDTOs = _serviceManager.List(query);
            ListModel<ServiceDTO> listModel = new ListModel<ServiceDTO>();
            if (serviceDTOs.Any())
            {
                listModel = new ListModel<ServiceDTO>()
                {
                    Data = serviceDTOs,
                    Total = serviceDTOs.First().TotalRecords
                };
            }
            else
            {
                listModel = new ListModel<ServiceDTO>()
                {
                    Data = serviceDTOs,
                    Total = 0
                };
            }


            return new JsonResult()
            {
                Data = listModel,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
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