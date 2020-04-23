using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Test")]
namespace Honeypot.Filter
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Extensions.Primitives;
    using Microsoft.AspNetCore.Http;
    using static Filter.StringHasher;
    using Microsoft.AspNetCore.Mvc;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc.Abstractions;
    using System.Threading.Tasks;

   
    internal class HoneypotService
    {
        private class FormField
        {
            public string HashedName { get; set; }
            public string UnhashedName { get; set; }
            public string Value { get; set; }
        }

        public static IFormCollection ValidateRequest(IFormCollection form)
        {
            var formFields = RemoveMaskedFields(form);
            var newForm = new FormCollection(formFields);
            return newForm;
        }

        public static async Task UpdateActionArguments(Controller ctrl, ActionDescriptor actionDescriptor, IDictionary<string, object> actionArguments)
        {
            foreach (var argument in actionArguments.ToList())
            {
                var parameter = Activator.CreateInstance(argument.Value.GetType());
                await ctrl.TryUpdateModelAsync(parameter, parameter.GetType(), string.Empty);
                var key = argument.Key;
                actionArguments[key] = parameter;
            }
        }


        private static Dictionary<string, StringValues> RemoveMaskedFields(IFormCollection form)
        {
            if (form is null) throw new ArgumentNullException("Form collection is empty");

            var dic = new Dictionary<string, StringValues>();
            var anyMaskFieldPresent = false;
            foreach (var field in form)
            {
                var isMaskedField = field.Key.StartsWith("mask-");
                var formCompromised = isMaskedField && !string.IsNullOrWhiteSpace(field.Value);
                if (isMaskedField) anyMaskFieldPresent = isMaskedField;
                if (formCompromised) throw new InvalidOperationException("Bot detected");
                var isNotMaskedField = (!isMaskedField && field.Key != "__RequestVerificationToken");
                if (isNotMaskedField) dic.Add(DecryptString(field.Key), field.Value);
            }
            if (!anyMaskFieldPresent) throw new InvalidOperationException("Mask field is not submited");
            return dic;
        }
    }
}
