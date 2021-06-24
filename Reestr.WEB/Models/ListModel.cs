using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Reestr.WEB.Models
{
    public class ListModel<T> where T : class
    {

        public List<T> Data { get; set; }

        public int Total { get; set; }

    }
}