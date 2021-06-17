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

        public List<string> ErrorMessages { get; set; }
        
        public string GetAllErrors()
        {            
            return string.Join(", ", ErrorMessages);                            
        }

    }
}
