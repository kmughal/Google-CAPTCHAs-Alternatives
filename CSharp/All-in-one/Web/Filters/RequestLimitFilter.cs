namespace Web.Filters
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Web.Services;

    public class RequestLimitFilter : ActionFilterAttribute
    {
        private readonly IRateLimitService _rateLimitService;

        public RequestLimitFilter(IRateLimitService rateLimitService)
        {
            _rateLimitService = rateLimitService;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var requestIpAddress = context.HttpContext.Connection.RemoteIpAddress;
            var isRequestValid = _rateLimitService.IsRequestValidToProceed(requestIpAddress);

            if (!isRequestValid) context.ModelState.AddModelError("REQUEST_LIMIT_ERROR", "Too many requests please wait...");
            base.OnActionExecuting(context);

        }
    }
}