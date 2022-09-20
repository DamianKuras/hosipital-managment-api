using hosipital_managment_api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace hosipital_managment_api.ActionFilters
{
    public class DepartmentValidationFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var departmentObject = context.ActionArguments.SingleOrDefault(p => p.Value is Department);
            if (departmentObject.Value == null)
            {
                context.Result = new BadRequestObjectResult("Departmnet value cannot be null");
                return;
            }
            if (!context.ModelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(context.ModelState);
            }
        }
    }
}
