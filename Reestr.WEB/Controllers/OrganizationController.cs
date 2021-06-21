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


        public ActionResult Index()
        {
            return View("Organizations");
        }


        public ActionResult List()
        {
            OrganizationQuery query = new OrganizationQuery();
            query.IsDeleted = false;
            query.Offset = 0;
            query.Limit = 20;
            List<OrganizationDTO> organizationDTOs = _organizationManager.List(query);

            return Json(organizationDTOs);
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