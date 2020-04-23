namespace Honeypot.Filter
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    public class HoneypotFilterAttribute : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var form = context.HttpContext.Request.Form;

            try
            {
                context.HttpContext.Request.Form = HoneypotService.ValidateRequest(form);
                var (ctrl, actionDescriptor, actionArguments) = (context.Controller as Controller, context.ActionDescriptor, context.ActionArguments);
                await HoneypotService.UpdateActionArguments(ctrl, actionDescriptor, actionArguments);
            }
            catch (InvalidOperationException ex)
            {
                context.ModelState.AddModelError("INVALID_REQUEST", ex.Message);
            }
            await next();
        }
    }
}