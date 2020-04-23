namespace Honeypot.Filter.TagHelpers
{
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using static Honeypot.Filter.StringHasher;

    public class HoneypotRadioboxTagHelper : TagHelper
    {
        public string Name { get; set; }

        public string Label { get; set; }

        public string Value { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var hashedFieldNameOrId = EncryptString(Name);
            
            var inlineStyle = " style ='opacity: 0;position: absolute;top: 0;left: 0;height: 0;width: 0;z-index: -1;'";
            var markup = $@"
                            <div {inlineStyle} class=""form-check"">
                                <input {inlineStyle} class=""form-check-input"" type=""radio"" value=""{Value}"" name=""mask-{Name}"">
                                <label {inlineStyle} class=""form-check-label"" for=""mask-{Name}"">
                                    {Label}
                                </label>
                            </div>
                            <div class=""form-check"">
                                <input class=""form-check-input"" type=""radio"" value=""{Value}"" name=""{hashedFieldNameOrId}"">
                                <label class=""form-check-label"" for=""{hashedFieldNameOrId}"">
                                    {Label}
                                </label>
                            </div>
                            ";

            output.TagName = "div";
            output.Attributes.SetAttribute("class", "form-group");
            output.Content.AppendHtml(markup);
        }
    }
}