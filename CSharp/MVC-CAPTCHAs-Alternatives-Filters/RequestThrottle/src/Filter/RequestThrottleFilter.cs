using System;
namespace RequestThrottle.Filter
{

    using Microsoft.AspNetCore.Mvc.Filters;

    public interface IRequestThrottleFilterAttribute : IActionFilter, IFilterMetadata, IAsyncActionFilter, IResultFilter, IAsyncResultFilter, IOrderedFilter {

    }

    public class RequestThrottleFilterAttribute : ActionFilterAttribute, IRequestThrottleFilterAttribute
    {
        private readonly IRequestThrottleService _requestThrottleService;

        public RequestThrottleFilterAttribute(IRequestThrottleService requestThrottleService)
        {
            _requestThrottleService = requestThrottleService;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var requestIpAddress = context.HttpContext.Connection.RemoteIpAddress;
            try
            {
                _requestThrottleService.IsRequestValidToProceed(requestIpAddress);
            }
            catch (InvalidOperationException e)
            {
                context.ModelState.AddModelError("REQUEST_LIMIT_ERROR", e.Message);
            }


            base.OnActionExecuting(context);
        }
    }
}