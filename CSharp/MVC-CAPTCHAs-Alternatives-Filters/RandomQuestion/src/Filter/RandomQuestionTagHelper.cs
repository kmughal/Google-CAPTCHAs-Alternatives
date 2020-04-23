namespace RandomQuestion.Filter
{
    using Microsoft.AspNetCore.Razor.TagHelpers;

    public class RandomQuestionTagHelper : TagHelper
    {

        public string PrefixLabel { set; get; }
        public string CssClass { set; get; }
        public string Placeholder { set; get; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            PrefixLabel ??= "Please answer this question :";
            CssClass ??= "form-group";
            Placeholder ??= "Please complete this";

            var qa = RandomQuestionService.GenerateRandomQuestion();
            var markup = $@"
            <label>{PrefixLabel}{qa.Question}</label>
            <input type=""hidden"" name=""random-question-id"" value=""{qa.Id}""/>
            <input type=""text"" class=""form-control"" name=""random-question"" id=""random-question"" placeholder=""{Placeholder}""/>
           ";
            output.TagName = "div";
            output.Attributes.SetAttribute("class", CssClass);
            output.Content.AppendHtml(markup);
        }
    }
}