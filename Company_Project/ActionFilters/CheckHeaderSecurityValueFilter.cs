using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Company_Project.ActionFilters
{
    public class CheckHeaderSecurityValueFilter: ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var httpRequest = context.HttpContext.Request;
            if (!httpRequest.Headers.TryGetValue("YourPasswordHeaderName", out var password) || password != "asd123AAA-DDD")
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}
