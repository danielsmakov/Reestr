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
    public class OrganizationController : Controller
    {
        private OrganizationManager _organizationManager;
        public OrganizationController()
        {
            _organizationManager = new OrganizationManager(new UnitOfWork());
        }

        public OrganizationController(OrganizationManager organizationManager)
        {
            _organizationManager = organizationManager;
        }

        [Route("/")]
        public ActionResult Index()
        {
            return View("Organizations");
        }


        [HttpGet]
        public JsonResult List(OrganizationQuery query)
        {
            /*OrganizationQuery query = new OrganizationQuery();
            query.IsDeleted = false;
            query.Offset = 0;
            query.Limit = 20;*/

            List<OrganizationDTO> organizationDTOs = _organizationManager.List(query);

            ListModel<OrganizationDTO> listModel = new ListModel<OrganizationDTO>()
            {
                Data = organizationDTOs,
                Total = organizationDTOs.First().TotalRecords
            };

            return new JsonResult()
            {
                Data = listModel,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }


        [HttpPost]
        public ActionResult Insert(OrganizationDTO organizationDTO)
        {
            var validationResponse = _organizationManager.Insert(organizationDTO);

            return Json(validationResponse);
        }


        [HttpPost]
        public ActionResult Update(OrganizationDTO organizationDTO)
        {
            var validationResponse = _organizationManager.Update(organizationDTO);

            return Json(validationResponse);
        }


        [HttpPost]
        public ActionResult Delete(OrganizationDTO organizationDTO)
        {
            var validationResponse = _organizationManager.Delete(organizationDTO.Id);

            return Json(validationResponse);
        }
    }
}