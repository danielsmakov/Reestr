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
        [Required(ErrorMessage = "Идентификатор организации не найден")]
        public int OrganizationId { get; set; }


        /// <summary>
        /// IIN состоит из 12 цифр
        /// </summary>
        [Required(ErrorMessage = "Необходимо указать ИИН")]
        [RegularExpression(@"^\d{12}", ErrorMessage = "Необходимо указать 12 цифр ИИН")]
        public string IIN { get; set; }


        /// <summary>
        /// FullName - Фамилия, имя и отчество
        /// </summary>
        [Required(ErrorMessage = "Необходимо указать полные ФИО")]
        [StringLength(150, MinimumLength = 8, ErrorMessage = "ФИО должно содержать от 8 до 150 символов")]
        public string FullName { get; set; }


        [Required(ErrorMessage = "Необходимо указать дату рождения")]
        public DateTime DateOfBirth { get; set; }


        [Required(ErrorMessage = "Необходимо указать номер телефона")]
        [Phone(ErrorMessage = "Номер телефона должен содержать 10 цифр, начинающихся после +7/8")]
        public string PhoneNumber { get; set; }


        [Required(ErrorMessage = "Необходимо указать дату начала")]
        public DateTime BeginDate { get; set; }
    }
}
