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

            return Json(serviceReestrDTOs, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Insert(ServiceReestrDTO serviceReestrDTO)
        {
            if (ModelState.IsValid)
            {
                var validationResponse = _serviceReestrManager.Insert(serviceReestrDTO);
                // TODO: отправить ошибки из validationResponse в UI
            }


            // TODO: узнать, в каком виде отправлять ответ из метода Insert
            return Json(new[] { serviceReestrDTO });
        }


        public ActionResult Update(ServiceReestrDTO serviceReestrDTO)
        {
            if (ModelState.IsValid)
            {
                var validationResponse = _serviceReestrManager.Update(serviceReestrDTO);
                // TODO: отправить ошибки из validationResponse в UI
            }

            // TODO: узнать, в каком виде отправлять ответ из метода Update
            return Json(new[] { serviceReestrDTO });
        }


        public ActionResult Delete(ServiceReestrDTO serviceReestrDTO)
        {
            var validationResponse = _serviceReestrManager.Delete(serviceReestrDTO.Id);
            // TODO: отправить ошибки из validationResponse в UI

            // TODO: узнать, в каком виде отправлять ответ из метода Delete
            return Json(new[] { serviceReestrDTO });
        }
    }
}