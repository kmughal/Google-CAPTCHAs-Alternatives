namespace Web.TagHelpers
{

    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Web.Services;
    public class RandomQuestionTagHelper : TagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var qa = RandomQuestionService.GenerateRandomQuestion();
           var markup = @$"
            <label>Please answer this question : {qa.Question}</label>
            <input type=""hidden"" name=""random-question-id"" value=""{qa.Id}""/>
            <input type=""text"" class=""form-control"" name=""random-question"" id=""random-question"" placeholder=""Please complete this""/>
           ";
            output.TagName = "div";
            output.Attributes.SetAttribute("class", "form-group");
            output.Content.AppendHtml(markup);
        }
    }
}