using System;
using System.Linq;
using System.Collections.Generic;
namespace Web.Filters
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.Filters;

    public class HoneypotFilter : ActionFilterAttribute
    {
        private class FormField
        {
            public string HashedName { get; set; }
            public string UnhashedName { get; set; }

            public string Value { get; set; }
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var form = context.HttpContext.Request.Form;
            var formIsCompromised = IsFormTempered(form);
            if (formIsCompromised) context.ModelState.AddModelError("FORM_COMPROMISED", "Bot detected!");
            else
            {
                var filterOutMaskedProperties = RemoveMaskedParameters(form);
                var unhashedProperties = UnhashParameternames(filterOutMaskedProperties);
                RemoveMaskedParameters(form, unhashedProperties);
            }
            base.OnActionExecuting(context);
        }

        private bool IsFormTempered(IFormCollection form)
        {
            foreach (var field in form)
            {
                var IsMaskedProperty = field.Key.StartsWith("mask-");
                var formCompromised = IsMaskedProperty && !string.IsNullOrWhiteSpace(field.Value);
                if (formCompromised) return true;
            }
            return default;
        }


        private Dictionary<string, string> RemoveMaskedParameters(IFormCollection form)
        {
            var dict = new Dictionary<string, string>();
            foreach (var field in form)
            {
                var IsNotMaskedField = !field.Key.StartsWith("mask-");
                if (IsNotMaskedField && field.Key != "__RequestVerificationToken") dict.Add(field.Key, field.Value);
            }
            return dict;
        }

        private List<FormField> UnhashParameternames(Dictionary<string, string> values)
        {
            var list = new List<FormField>();
            foreach (var item in values)
            {
                var newItem = new FormField
                {
                    HashedName = item.Key,
                    UnhashedName = Web.Helpers.StringHasher.DecryptString(item.Key),
                    Value = item.Value
                };
                list.Add(newItem);
            }
            return list;
        }

        private void RemoveMaskedParameters(IFormCollection form, List<FormField> values)
        {
            var dataset = new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>();
            foreach (var value in values)
            {
                dataset.Add(value.UnhashedName, value.Value);
            }
            form = new FormCollection(dataset);
        }
    }
}