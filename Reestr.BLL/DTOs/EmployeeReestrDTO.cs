using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reestr.BLL.DTOs
{
    public class EmployeeReestrDTO
    {
        public int Id { get; set; }

        /// <summary>
        /// Id of the Organization to which Employee belongs
        /// </summary>
        public int OrganizationId { get; set; }

        /// <summary>
        /// IIN - Individual Identification Number, consists of 12 digits
        /// </summary>
        public string IIN { get; set; }

        /// <summary>
        /// Full name - Фамилия, имя и отчество
        /// </summary>
        public string FullName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime BeginDate { get; set; }
    }
}
