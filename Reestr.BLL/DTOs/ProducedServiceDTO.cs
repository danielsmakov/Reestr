using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reestr.BLL.DTOs
{
    public class ProducedServiceDTO
    {

        public int Id { get; set; }


        /// <summary>
        /// Getting from Organizations Table, ReestrDatabase 
        /// </summary>
        [Required(ErrorMessage = "Необходимо указать организацию")]
        public int OrganizationId { get; set; }


        /// <summary>
        /// Getting from Servces Table, ReestrDatabase
        /// </summary>
        [Required(ErrorMessage = "Необходимо указать услугу")]
        public int ServiceReestrId { get; set; }


        /// <summary>
        /// Getting from EmplolyeeReestr Table, ReestrDatabase
        /// </summary>
        [Required(ErrorMessage = "Необходимо указать сотрудника")]
        public int EmployeeReestrId { get; set; }


        [Required(ErrorMessage = "Необходимо указать дату начала")]
        public DateTime BeginDate { get; set; }
    }
}
