﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reestr.BLL.DTOs
{
    public class ServiceReestrDTO
    {
        public int Id { get; set; }

        public int OrganizationId { get; set; }

        public int ServiceId { get; set; }

        public decimal Price { get; set; }

        public DateTime BeginDate { get; set; }
    }
}
