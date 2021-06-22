using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;


namespace Reestr.BLL.DTOs
{
    public class OrganizationDTO
    {

        public int Id { get; set; }


        /*[Required(ErrorMessage = "Не указано название организации")]*/
        [Required(ErrorMessageResourceType = typeof(Resources.Resources_ru), ErrorMessageResourceName = "Required")]
        [StringLength(300, MinimumLength = 2, ErrorMessageResourceType = typeof(Resources.Resources_ru), ErrorMessageResourceName = "NameLength")]
        public string Name { get; set; }


        /// <summary>
        /// BIN (БИН) состоит из 12 цифр и является уникальным идентификатором организации
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(Resources.Resources_ru), ErrorMessageResourceName = "Required")]
        [RegularExpression(@"^\d{12}", ErrorMessageResourceType = typeof(Resources.Resources_ru), ErrorMessageResourceName = "BINAndIINLength")]
        public string BIN { get; set; }


        [Required(ErrorMessageResourceType = typeof(Resources.Resources_ru), ErrorMessageResourceName = "Required")]
        [RegularExpression(@"^\d{10}", ErrorMessageResourceType = typeof(Resources.Resources_ru), ErrorMessageResourceName = "PhoneNumberLength")]
        public string PhoneNumber { get; set; }


        [Required(ErrorMessageResourceType = typeof(Resources.Resources_ru), ErrorMessageResourceName = "Required")]
        public DateTime BeginDate { get; set; }

    }
}
