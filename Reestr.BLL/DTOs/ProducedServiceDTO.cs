using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reestr.BLL.DTOs
{
    class ProducedServiceDTO
    {
        public int Id { get; set; }

        /// <summary>
        /// Id of organization which produced the service
        /// </summary>
        public int OrganizationId { get; set; }

        /// <summary>
        /// Id of produced service
        /// </summary>
        public int ServiceId { get; set; }

        /// <summary>
        /// Id of employee which produced the service
        /// </summary>
        public int EmployeeId { get; set; }

        public DateTime BeginDate { get; set; }
    }
}
