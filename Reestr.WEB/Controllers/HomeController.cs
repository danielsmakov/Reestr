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
    public class HomeController : Controller
    {
        public static class TestData
        {
            public static List<OrganizationDTO> GetListOfOrganizationDTOs()
            {
                OrganizationDTO entity = new OrganizationDTO()
                {
                    Name = "Google",
                    BIN = "123123123123",
                    PhoneNumber = "7475065068",
                    BeginDate = DateTime.Now
                };
                List<OrganizationDTO> organizationDTOs = new List<OrganizationDTO>();
                for (int i = 0; i < 6; i++)
                {
                    organizationDTOs.Add(entity);
                }
                return organizationDTOs;
            }
            public static OrganizationDTO GetOrganizationDTO()
            {
                OrganizationDTO organizationDTO = new OrganizationDTO()
                {
                    Name = "Oracle",
                    BIN = "555555555555",
                    PhoneNumber = "7475065068",
                    BeginDate = DateTime.Now
                };
                return organizationDTO;
            }
            public static OrganizationDTO GetOrganizationDTOWithId()
            {
                OrganizationDTO organizationDTO = new OrganizationDTO()
                {
                    Id = 3,
                    Name = "Microsoft",
                    BIN = "333333333333",
                    PhoneNumber = "8888888888",
                    BeginDate = DateTime.Now
                };
                return organizationDTO;
            }
        }
        private OrganizationManager _organizationManager;
        public HomeController()
        {
            _organizationManager = new OrganizationManager(new UnitOfWork());
        }

        public HomeController(OrganizationManager organizationManager)
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
            query.IsDeleted = true;
            query.Offset = 0;
            query.Limit = 20;
            List<OrganizationDTO> organizationDTOs = _organizationManager.List(query);
            if (organizationDTOs.Count == 0)
            {
                ViewBag.ErrorMessage = "No model found.";
                return View("Error");
            }
            ViewBag.SuccessMessage = $"Number of Models in Database is {organizationDTOs.Count}!";
            return View("Success");
        }
        public ActionResult Insert()
        {
            var organizationDTO = TestData.GetOrganizationDTO();
            var validationResponse = _organizationManager.Insert(organizationDTO);
            if (!validationResponse.Status)
            {
                ViewBag.ErrorMessage = validationResponse.ErrorMessage;
                return View("Error");
            }
            ViewBag.SuccessMessage = $"Successfully Inserted {organizationDTO.Name} Organization to Database!";
            return View("Success");
        }
        public ActionResult Update()
        {
            var organizationDTO = TestData.GetOrganizationDTOWithId();
            var validationResponse = _organizationManager.Update(organizationDTO);
            if (!validationResponse.Status)
            {
                ViewBag.ErrorMessage = validationResponse.ErrorMessage;
                return View("Error");
            }
            ViewBag.SuccessMessage = $"Successfully Updated {organizationDTO.Name} Organization!";
            return View("Success");
        }
        public ActionResult Delete()
        {
            var validationResponse = _organizationManager.Delete(15);
            if (!validationResponse.Status)
            {
                ViewBag.ErrorMessage = validationResponse.ErrorMessage;
                return View("Error");
            }
            ViewBag.SuccessMessage = "Organization Is Successfully Deleted!";
            return View("Success");
        }





        // Ниже методы для Kendo UI View
        /*public ActionResult List([DataSourceRequest] DataSourceRequest request)
        {
            OrganizationQuery query = new OrganizationQuery();
            query.Offset = (request.Page - 1) * request.PageSize;
            query.Limit = 20;
            List<OrganizationDTO> organizationDTOs = _organizationManager.List(query);
            return Json(organizationDTOs.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Insert([DataSourceRequest] DataSourceRequest request, OrganizationDTO organizationDTO)
        {
            if (organizationDTO != null && ModelState.IsValid)
            {
                if (!_organizationManager.Insert(organizationDTO))
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
        }*/

    }
}