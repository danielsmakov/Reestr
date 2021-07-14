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


        public ActionResult List(ServiceReestrQuery query)
        {
            List<ServiceReestrDTO> serviceReestrDTOs = _serviceReestrManager.List(query);
            ListModel<ServiceReestrDTO> listModel = new ListModel<ServiceReestrDTO>();
            if (serviceReestrDTOs.Any())
            {
                listModel = new ListModel<ServiceReestrDTO>
                {
                    Data = serviceReestrDTOs,
                    Total = serviceReestrDTOs.First().TotalRecords
                };
            }
            else
            {
                listModel = new ListModel<ServiceReestrDTO>
                {
                    Data = serviceReestrDTOs,
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

        public ActionResult Edit(int? id)
        {
            if (id == 0)
            {
                return PartialView("Edit");
            }

            return PartialView("Edit");
        }
    }
}