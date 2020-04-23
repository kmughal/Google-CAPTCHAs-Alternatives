namespace Web.TagHelpers
{

    using Microsoft.AspNetCore.Razor.TagHelpers;
    using static Web.Helpers.StringHasher;

    public class HoneypotFieldTagHelper : TagHelper
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

            var markup = $@"
            <label for=""mask-{Name}"" class=""mask"">{Label}</label>
            <input class=""mask form-control"" type=""{Type}"" id=""mask-{Name}"" name=""mask-{Name}"" placeholder=""{Placeholder}""/>

            <label for=""{hashedFieldNameOrId}"">{Label}</label>
            <input type=""{Type}"" class=""form-control"" id=""{hashedFieldNameOrId}"" name=""{hashedFieldNameOrId}"" placeholder=""{Placeholder}""/>

            ";
            output.TagName = "div";
            output.Attributes.SetAttribute("class", "form-group");
            output.Content.AppendHtml(markup);
        }
    }
}