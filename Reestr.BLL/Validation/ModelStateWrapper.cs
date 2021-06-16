using Reestr.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Reestr.BLL.Validation
{
    public class ModelStateWrapper
    {
        private ModelStateDictionary _modelState;
        public ModelStateWrapper(ModelStateDictionary modelState)
        {
            _modelState = modelState;
        }
        public void AddError(string key, string errorMessage)
        {
            _modelState.AddModelError(key, errorMessage);
        }
        public bool IsValid
        {
            get => _modelState.IsValid;
        }
    }
}
