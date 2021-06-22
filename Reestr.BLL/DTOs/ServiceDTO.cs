using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reestr.BLL.DTOs
{
    public class ServiceDTO
    {

        public int Id { get; set; }


        [Required(ErrorMessageResourceType = typeof(Resources.Resources_ru), ErrorMessageResourceName = "Required")]
        [StringLength(300, MinimumLength = 2, ErrorMessageResourceType = typeof(Resources.Resources_ru), ErrorMessageResourceName = "NameLength")]
        public string Name { get; set; }


        /// <summary>
        /// Code состоит из буквы от A-Z, 2 чисел, точки, 3 чисел, точки, 3 чисел (A32.123.321)
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(Resources.Resources_ru), ErrorMessageResourceName = "Required")]
        [RegularExpression(@"^[A-Z]{1}\d{2}\.\d{3}\.\d{3}", ErrorMessageResourceType = typeof(Resources.Resources_ru), ErrorMessageResourceName = "CodeRegex")]
        public string Code { get; set; }


        [Required(ErrorMessageResourceType = typeof(Resources.Resources_ru), ErrorMessageResourceName = "Required")]
        [RegularExpression(@"^[1-9]\d*$", ErrorMessageResourceType = typeof(Resources.Resources_ru), ErrorMessageResourceName = "PriceRegex")]
        public decimal Price { get; set; }


        [Required(ErrorMessageResourceType = typeof(Resources.Resources_ru), ErrorMessageResourceName = "Required")]
        public DateTime BeginDate { get; set; }
    }
}
