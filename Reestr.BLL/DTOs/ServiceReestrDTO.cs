using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reestr.BLL.DTOs
{
    public class ServiceReestrDTO
    {
        public int Id { get; set; }


        [Required(ErrorMessage = "Необходимо указать организацию")]
        public int OrganizationId { get; set; }


        [Required(ErrorMessage = "Необходимо указать услугу")]
        public int ServiceId { get; set; }


        [Required(ErrorMessage = "Необходимо указать цену")]
        [RegularExpression(@"^[1-9]\d*$", ErrorMessage = "Указанная сумма не должна быть меньше 0")]
        public decimal Price { get; set; }


        [Required(ErrorMessage = "Необходимо указать дату начала")]
        public DateTime BeginDate { get; set; }
    }
}
