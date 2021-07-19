using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reestr.BLL.DTOs
{
    public class ServiceReestrDTO
    {
        public int Id { get; set; }


        [Required(ErrorMessageResourceType = typeof(Resources.Resources_ru), ErrorMessageResourceName = "Required")]
        public int OrganizationId { get; set; }

        public OrganizationDTO Organization { get; set; }


        [Required(ErrorMessageResourceType = typeof(Resources.Resources_ru), ErrorMessageResourceName = "Required")]
        public int ServiceId { get; set; }

        public ServiceDTO Service { get; set; }


        [Required(ErrorMessageResourceType = typeof(Resources.Resources_ru), ErrorMessageResourceName = "Required")]
        [RegularExpression(@"^[1-9]\d*$", ErrorMessageResourceType = typeof(Resources.Resources_ru), ErrorMessageResourceName = "PriceRegex")]
        public decimal Price { get; set; }


        [Required(ErrorMessageResourceType = typeof(Resources.Resources_ru), ErrorMessageResourceName = "Required")]
        public DateTime BeginDate { get; set; }

        public int TotalRecords { get; set; }
    }
}
