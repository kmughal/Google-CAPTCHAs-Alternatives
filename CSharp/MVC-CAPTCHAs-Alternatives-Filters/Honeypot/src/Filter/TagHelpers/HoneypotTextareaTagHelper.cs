namespace Honeypot.Filter.TagHelpers
{
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using static Honeypot.Filter.StringHasher;

    public class HoneypotTextareaTagHelper : TagHelper
    {
        public string Name { get; set; }

        public string Label { get; set; }

        public string Placeholder { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var hashedFieldNameOrId = EncryptString(Name);

            Placeholder = Placeholder ?? string.Empty;
            var inlineStyle = " style ='opacity: 0;position: absolute;top: 0;left: 0;height: 0;width: 0;z-index: -1;'";
            var markup = $@"
            <label  for=""mask-{Name}"" {inlineStyle}>{Label}</label>
            <Textarea {inlineStyle} id=""mask-{Name}"" name=""mask-{Name}"" placeholder=""{Placeholder}""></Textarea>
            <label for=""{hashedFieldNameOrId}"">{Label}</label>
            <Textarea class=""form-control"" id=""{hashedFieldNameOrId}"" name=""{hashedFieldNameOrId}"" placeholder=""{Placeholder}""></Textarea>
            ";
            output.TagName = "div";
            output.Attributes.SetAttribute("class", "form-group");
            output.Content.AppendHtml(markup);
        }
    }
}