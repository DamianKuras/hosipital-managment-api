using hosipital_managment_api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace hosipital_managment_api.ActionFilters
{
    public class MedicineValidationFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var departmentObject = context.ActionArguments.SingleOrDefault(p => p.Value is Medicine);
            if (departmentObject.Value == null)
            {
                context.Result = new BadRequestObjectResult("Medicine value cannot be null");
                return;
            }
            if (!context.ModelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(context.ModelState);
            }
        }
    }
}
