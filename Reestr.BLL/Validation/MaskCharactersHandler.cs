using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Reestr.BLL.Validation
{
    public static class MaskCharactersHandler
    {
        public static string RemoveEveryCharacterExceptForDigits(string phoneNumber)
        {
            string pattern = @"\D";
            string target = "";
            Regex phoneRegex = new Regex(pattern);
            phoneNumber = phoneRegex.Replace(phoneNumber, target);

            return phoneNumber;
        }
    }
}
