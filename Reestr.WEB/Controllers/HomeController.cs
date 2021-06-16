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
    public class HomeController : Controller
    {
        private OrganizationManager _organizationManager;
        public HomeController()
        {
            _organizationManager = new OrganizationManager(new ModelStateWrapper(this.ModelState), new UnitOfWork());
        }
        public HomeController(OrganizationManager organizationManager)
        {
            _organizationManager = organizationManager;
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult List([DataSourceRequest] DataSourceRequest request)
        {
            OrganizationQuery query = new OrganizationQuery();
            query.Offset = (request.Page - 1) * request.PageSize;
            query.Limit = request.PageSize;
            List<OrganizationDTO> organizationDTOs = _organizationManager.List(query);
            return Json(organizationDTOs.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public ActionResult Insert([DataSourceRequest] DataSourceRequest request, OrganizationDTO organizationDTO)
        {
            if (organizationDTO != null && ModelState.IsValid)
            {
                if (_organizationManager.Insert(organizationDTO))
                    return Json(new[] { organizationDTO }.ToDataSourceResult(request, ModelState));
            }
            return Json(new[] { organizationDTO }.ToDataSourceResult(request, ModelState));
        }
        public ActionResult Update([DataSourceRequest] DataSourceRequest request, OrganizationDTO organizationDTO)
        {
            if (organizationDTO != null && ModelState.IsValid)
            {
                if (_organizationManager.Update(organizationDTO))
                    return Json(new[] { organizationDTO }.ToDataSourceResult(request, ModelState));

            }
            return Json(new[] { organizationDTO }.ToDataSourceResult(request, ModelState));
        }
        public ActionResult Delete([DataSourceRequest] DataSourceRequest request, OrganizationDTO organizationDTO)
        {
            if (organizationDTO != null)
            {
                if (_organizationManager.Delete(organizationDTO.Id))
                    ModelState.AddModelError("Deletion", "Can not delete this record");
                    return Json(new[] { organizationDTO }.ToDataSourceResult(request, ModelState));
            }
            return Json(new[] { organizationDTO }.ToDataSourceResult(request, ModelState));
        }
    }
}