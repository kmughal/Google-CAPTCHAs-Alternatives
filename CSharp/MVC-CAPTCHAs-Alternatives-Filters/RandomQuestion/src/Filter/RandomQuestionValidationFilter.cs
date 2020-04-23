namespace RandomQuestion.Filter
{
    using System;
    using Microsoft.AspNetCore.Mvc.Filters;

    public class RandomQuestionValidationFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var form = context.HttpContext.Request.Form;

            try
            {
                RandomQuestionService.ValidateProvidedAnswer(form);
            }
            catch (InvalidOperationException ex)
            {
                context.ModelState.AddModelError("INVALID_REQUEST", ex.Message);
            }

            base.OnActionExecuting(context);
        }
    }
}