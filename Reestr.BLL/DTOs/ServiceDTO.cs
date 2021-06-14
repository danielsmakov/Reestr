using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reestr.BLL.DTOs
{
    class ServiceDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// Code consists of 1 letter from A-Z and 8 digits
        /// </summary>
        public string Code { get; set; }

        public decimal Price { get; set; }

        public DateTime BeginDate { get; set; }
    }
}
