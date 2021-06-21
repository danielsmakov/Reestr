﻿using System;
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
    public class ProducedServiceController : Controller
    {
        private ProducedServiceManager _producedServiceManager;
        public ProducedServiceController()
        {
            _producedServiceManager = new ProducedServiceManager(new UnitOfWork());
        }


        public ProducedServiceController(ProducedServiceManager producedServiceManager)
        {
            _producedServiceManager = producedServiceManager;
        }


        public ActionResult Index()
        {
            return View("ProducedService");
        }


        public ActionResult List()
        {
            ProducedServiceQuery query = new ProducedServiceQuery();
            query.Offset = 0;
            query.Limit = 20;
            List<ProducedServiceDTO> producedServiceDTOs = _producedServiceManager.List(query);

            return Json(producedServiceDTOs, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Insert(ProducedServiceDTO producedServiceDTO)
        {
            if (ModelState.IsValid)
            {
                var validationResponse = _producedServiceManager.Insert(producedServiceDTO);
                // TODO: отправить ошибки из validationResponse в UI
            }


            // TODO: узнать, в каком виде отправлять ответ из метода Insert
            return Json(new[] { producedServiceDTO });
        }


        public ActionResult Update(ProducedServiceDTO producedServiceDTO)
        {
            if (ModelState.IsValid)
            {
                var validationResponse = _producedServiceManager.Update(producedServiceDTO);
                // TODO: отправить ошибки из validationResponse в UI
            }

            // TODO: узнать, в каком виде отправлять ответ из метода Update
            return Json(new[] { producedServiceDTO });
        }


        public ActionResult Delete(ProducedServiceDTO producedServiceDTO)
        {
            var validationResponse = _producedServiceManager.Delete(producedServiceDTO.Id);
            // TODO: отправить ошибки из validationResponse в UI

            // TODO: узнать, в каком виде отправлять ответ из метода Delete
            return Json(new[] { producedServiceDTO });
        }
    }
}