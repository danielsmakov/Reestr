using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reestr.BLL.DTOs
{
    public class EmployeeReestrDTO
    {
        
        public int Id { get; set; }


        /// <summary>
        /// Id Организации к которой прикреплён сотрудник
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(Resources.Resources_ru), ErrorMessageResourceName = "Required")]
        public int OrganizationId { get; set; }


        /// <summary>
        /// IIN состоит из 12 цифр
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(Resources.Resources_ru), ErrorMessageResourceName = "Required")]
        [RegularExpression(@"^\d{12}", ErrorMessageResourceType = typeof(Resources.Resources_ru), ErrorMessageResourceName = "BINAndIINLength")]
        public string IIN { get; set; }


        /// <summary>
        /// FullName - Фамилия, имя и отчество
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(Resources.Resources_ru), ErrorMessageResourceName = "Required")]
        [StringLength(150, MinimumLength = 8, ErrorMessageResourceType = typeof(Resources.Resources_ru), ErrorMessageResourceName = "FullNameLength")]
        public string FullName { get; set; }


        [Required(ErrorMessageResourceType = typeof(Resources.Resources_ru), ErrorMessageResourceName = "Required")]
        public DateTime DateOfBirth { get; set; }


        [Required(ErrorMessageResourceType = typeof(Resources.Resources_ru), ErrorMessageResourceName = "Required")]
        [RegularExpression(@"^\d{10}", ErrorMessageResourceType = typeof(Resources.Resources_ru), ErrorMessageResourceName = "PhoneNumberLength")]
        public string PhoneNumber { get; set; }


        [Required(ErrorMessageResourceType = typeof(Resources.Resources_ru), ErrorMessageResourceName = "Required")]
        public DateTime BeginDate { get; set; }
    }
}
