namespace Honeypot.Filter.TagHelpers
{
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using static Honeypot.Filter.StringHasher;
    
    public class HoneypotTextboxTagHelper : TagHelper
    {
        public string Name { get; set; }

        public string Label { get; set; }

        public string Placeholder { get; set; }

        public string Type { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var hashedFieldNameOrId = EncryptString(Name);
            Type = Type ?? "text";
            Placeholder = Placeholder ?? string.Empty;
            var inlineStyle = " style ='opacity: 0;position: absolute;top: 0;left: 0;height: 0;width: 0;z-index: -1;'";
            var markup = $@"
            <label  for=""mask-{Name}"" {inlineStyle}>{Label}</label>
            <input {inlineStyle} type=""{Type}"" id=""mask-{Name}"" name=""mask-{Name}"" placeholder=""{Placeholder}""/>
            <label for=""{hashedFieldNameOrId}"">{Label}</label>
            <input type=""{Type}"" class=""form-control"" id=""{hashedFieldNameOrId}"" name=""{hashedFieldNameOrId}"" placeholder=""{Placeholder}""/>
            ";
            output.TagName = "div";
            output.Attributes.SetAttribute("class", "form-group");
            output.Content.AppendHtml(markup);
        }
    }
}