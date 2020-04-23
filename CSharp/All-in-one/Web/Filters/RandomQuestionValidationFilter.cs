namespace Web.Filters
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Web.Services;

    public class RandomQuestionValidationFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var form = context.HttpContext.Request.Form;
            var (randomQuestionPresent, providedAnswer, questionIdPresent, questionId) = RandomQuestionPresent(form);
            if (!randomQuestionPresent || !questionIdPresent) context.ModelState.AddModelError("INVALID_REQUEST", "Please provide the answer!");
            if (randomQuestionPresent)
            {
                var isAnswerValid = IsProvidedAnswerCorrect(questionId, providedAnswer);
                if (!isAnswerValid) context.ModelState.AddModelError("INVALID_REQUEST", "Provided answer is not correct!");
            }

            base.OnActionExecuting(context);
        }

        private (bool randomQuestionPresent, Microsoft.Extensions.Primitives.StringValues providedAnswer, bool questionIdPresent, string questionId) RandomQuestionPresent(IFormCollection form)
        {
            var randomQuestionPresent = form.TryGetValue("random-question", out Microsoft.Extensions.Primitives.StringValues providedAnswer);
            var questionIdPresent = form.TryGetValue("random-question-id", out Microsoft.Extensions.Primitives.StringValues questionId);

            return (randomQuestionPresent, providedAnswer, questionIdPresent, questionId);
        }

        private bool IsProvidedAnswerCorrect(string questionId, string providedAnswer)
        {
            var result = RandomQuestionService.ValidateProvidedAnswer(questionId, providedAnswer);
            return result;
        }
    }
}