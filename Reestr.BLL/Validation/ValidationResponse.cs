using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reestr.BLL.Validation
{
    public class ValidationResponse
    {

        public bool Status { get; set; } = true;

        public string ErrorMessage { get; set; } = string.Empty;
    }
}
