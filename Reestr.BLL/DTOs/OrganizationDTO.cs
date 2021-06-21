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

        [Required]
        public string Name { get; set; }

        /// <summary>
        /// BIN is a 12-digit unique company identifier
        /// </summary>
        
        [Required]
        public string BIN { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public DateTime BeginDate { get; set; }
    }
}
