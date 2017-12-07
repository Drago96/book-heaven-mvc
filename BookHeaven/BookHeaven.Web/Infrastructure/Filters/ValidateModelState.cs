using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace BookHeaven.Web.Infrastructure.Filters
{
    public class ValidateModelState : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var controller = context.Controller as Controller;
                var model = context.ActionArguments.FirstOrDefault(a => a.Key.ToLower().Contains("model")).Value;

                if (controller == null || model == null)
                {
                    return;
                }

                context.Result = controller.View(model);
            }
            base.OnActionExecuting(context);
        }
    }
}