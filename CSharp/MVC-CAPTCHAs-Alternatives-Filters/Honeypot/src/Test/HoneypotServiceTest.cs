using System.Linq;
using System.Collections.Generic;
namespace Test
{
    using System;
    using Xunit;
    using Honeypot.Filter;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Primitives;

    public class HoneypotService_ValidateRequest_Tests
    {
        [Fact]
        public void EmptyFormCollection_Should_ErrorOut()
        {
            Assert.Throws<ArgumentNullException>(() => Honeypot.Filter.HoneypotService.ValidateRequest(default));
        }

        [Fact]
        public void ValueProvidedForMaskedField_Should_ErrorOut()
        {
            var dic = new Dictionary<string, StringValues>();
            dic.Add("mask-name", "khurram");
            var form = new FormCollection(dic);

            Assert.Throws<InvalidOperationException>(() => Honeypot.Filter.HoneypotService.ValidateRequest(form));
        }

        [Fact]
        public void NoMaskedFieldUpdated_Should_BeFine()
        {
            var dic = new Dictionary<string, StringValues>();
            dic.Add(StringHasher.EncryptString("name"), "khurram");
            dic.Add("mask-age", "");
            var form = new FormCollection(dic);

            var newForm = Honeypot.Filter.HoneypotService.ValidateRequest(form);

            Assert.NotNull(newForm);
            Assert.False(newForm.Keys.Contains("mask-age"));
            Assert.True(newForm.Keys.Contains("name"));
        }


    }
}
