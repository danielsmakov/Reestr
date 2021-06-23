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
            return View("ProducedServices");
        }


        public ActionResult List()
        {
            ProducedServiceQuery query = new ProducedServiceQuery();
            query.Offset = 0;
            query.Limit = 20;
            List<ProducedServiceDTO> producedServiceDTOs = _producedServiceManager.List(query);

            return Json(producedServiceDTOs);
        }

        [HttpPost]
        public ActionResult Insert(ProducedServiceDTO producedServiceDTO)
        {
            var validationResponse = _producedServiceManager.Insert(producedServiceDTO);
            
            return Json(validationResponse);
        }

        [HttpPost]
        public ActionResult Update(ProducedServiceDTO producedServiceDTO)
        {
            var validationResponse = _producedServiceManager.Update(producedServiceDTO);
            
            return Json(validationResponse);
        }

        [HttpPost]
        public ActionResult Delete(ProducedServiceDTO producedServiceDTO)
        {
            var validationResponse = _producedServiceManager.Delete(producedServiceDTO.Id);
            
            return Json(validationResponse);
        }
    }
}