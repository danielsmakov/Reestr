using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reestr.BLL.DTOs
{
    public class OrganizationDTO
    {

        public int Id { get; set; }


        [Required(ErrorMessage = "Не указано название организации")]
        [StringLength(300, MinimumLength = 2, ErrorMessage = "Название организации не может быть короче 3 символов и превышать 300 ")]
        public string Name { get; set; }


        /// <summary>
        /// BIN (БИН) состоит из 12 цифр и является уникальным идентификатором организации
        /// </summary>
        [Required(ErrorMessage = "БИН не указан")]
        [StringLength(12, ErrorMessage = "БИН должен состоять из 12 цифр")]
        public string BIN { get; set; }


        [Required(ErrorMessage = "Необходимо указать номер телефона")]
        [StringLength(10, ErrorMessage = "Номер телефона должен содержать 10 цифр, начинающихся после +7/8")]
        public string PhoneNumber { get; set; }


        [Required(ErrorMessage = "Необходимо указать дату начала")]
        public DateTime BeginDate { get; set; }

    }
}
