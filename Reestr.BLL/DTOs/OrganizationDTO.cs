using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reestr.BLL.DTOs
{
    class OrganizationDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// BIN is a 12-digit unique company identifier
        /// </summary>
        public string BIN { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime BeginDate { get; set; }
    }
}
