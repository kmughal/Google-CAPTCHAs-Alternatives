namespace Honeypot.Filter.TagHelpers
{
    using System.Collections.Generic;
    using System.Text;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using static Honeypot.Filter.StringHasher;

    public class HoneypotSelectTagHelper : TagHelper
    {
        public class SelectItem
        {
            public string Text { get; set; }
            public string Value { get; set; }
        }

        public IEnumerable<object> List { get; set; }

        public string Name { get; set; }

        public string Label { get; set; }

        //public string Value { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var hashedFieldNameOrId = EncryptString(Name);

            var inlineStyle = " style ='opacity: 0;position: absolute;top: 0;left: 0;height: 0;width: 0;z-index: -1;'";

            var options = new StringBuilder();
            foreach (var item in List)
            {
                var toSelectItem = item as SelectItem;
                if (toSelectItem != null) options.AppendFormat(@"<option value=""{0}"">{1}</option>", toSelectItem.Value, toSelectItem.Text);
            }
            var maskedOptions = @$"{options.ToString()}<option value="""" selected disabled hidden>Select value</option>";
            var markup = $@"

                            <div {inlineStyle} class=""form-group"">
                                <label for=""mask-{Name}"">{Label}</label>
                                <select  class=""form-control"" name=""mask-{Name}"" id=""mask-{Name}"">
                                  {maskedOptions}
                                </select>
                            </div>

                           <div class=""form-group"">
                                <label for=""{hashedFieldNameOrId}"">{Label}</label>
                                <select  class=""form-control"" name=""{hashedFieldNameOrId}"" id=""{hashedFieldNameOrId}"">
                                  {options}
                                </select>
                            </div>
                            ";

            output.Content.AppendHtml(markup);
            output.TagName = "div";
            output.Attributes.SetAttribute("class", "form-group");
        }
    }
}