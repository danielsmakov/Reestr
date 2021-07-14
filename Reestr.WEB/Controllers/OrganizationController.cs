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

            List<OrganizationDTO> organizationDTOs = _organizationManager.List(query);
            ListModel<OrganizationDTO> listModel = new ListModel<OrganizationDTO>();
            if (organizationDTOs.Any())
            {
                listModel = new ListModel<OrganizationDTO>
                {
                    Data = organizationDTOs,
                    Total = organizationDTOs.First().TotalRecords
                };
            }
            else
            {
                listModel = new ListModel<OrganizationDTO>
                {
                    Data = organizationDTOs,
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
        public ActionResult Delete(int id)
        {
            var validationResponse = _organizationManager.Delete(id);

            return Json(validationResponse);
        }


        // AddOneRecord и AddRecords использовались для наполнения базы данных записями
        /*public void AddOneRecord()
        {
            Random random = new Random();
            OrganizationDTO serviceDTO = new OrganizationDTO
            {
                Name = "Organization " + $"{1}",
                BIN = $"{random.Next(100000, 999999)}" + $"{random.Next(100000, 999999)}",
                PhoneNumber = $"{random.Next(10000, 99999)}" + $"{random.Next(10000, 99999)}",
                BeginDate = DateTime.Now
            };
            _organizationManager.Insert(serviceDTO);
        }
        public void AddRecords()
        {
            Random random = new Random();
            for (int i = 2; i <= 1000; i++)
            {
                OrganizationDTO serviceDTO = new OrganizationDTO
                {
                    Name = "Organization " + $"{i}",
                    BIN = $"{random.Next(100000, 999999)}" + $"{random.Next(100000, 999999)}",
                    PhoneNumber = $"{random.Next(10000, 99999)}" + $"{random.Next(10000, 99999)}",
                    BeginDate = DateTime.Now
                };
                _organizationManager.Insert(serviceDTO);
            }
        }*/
    }
}