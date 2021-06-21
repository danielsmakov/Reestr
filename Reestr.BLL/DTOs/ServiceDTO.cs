using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reestr.BLL.DTOs
{
    public class ServiceDTO
    {

        public int Id { get; set; }


        [Required(ErrorMessage = "Необходимо указать название услуги")]
        [StringLength(300, MinimumLength = 3, ErrorMessage = "Название услуги не может быть короче 3 символов и длиннее 300")]
        public string Name { get; set; }


        /// <summary>
        /// Code состоит из буквы от A-Z, 2 чисел, точки, 3 чисел, точки, 3 чисел (A32.123.321)
        /// </summary>
        [Required(ErrorMessage = "Необходимо указать код с точками, как показано в строке заполнения")]
        [RegularExpression(@"^[A-Z]{1}\d{2}\.\d{3}\.\d{3}", ErrorMessage = "Необходимо ввести код в формате A12.123.123 с указанием точек, первая буква в диапазоне от A до Z")]
        public string Code { get; set; }


        [Required(ErrorMessage = "Необходимо указать цену")]
        [RegularExpression(@"^[1-9]\d*$", ErrorMessage = "Указанная сумма не должна быть меньше 0")]
        public decimal Price { get; set; }


        [Required(ErrorMessage = "Необходимо указать дату начала")]
        public DateTime BeginDate { get; set; }
    }
}
