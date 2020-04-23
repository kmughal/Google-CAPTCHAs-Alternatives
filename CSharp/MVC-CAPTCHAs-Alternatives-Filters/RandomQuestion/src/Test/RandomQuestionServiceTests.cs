namespace RandomQuestion.Filter.Test
{
    using System.Collections.Generic;
    using System;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Primitives;
    using Xunit;


    public class RandomQuestionService_ValidateProvidedAnswerTests
    {

        public RandomQuestionService_ValidateProvidedAnswerTests()
        {
            // this is to create the json
            try
            {
                var dict = CreateFormCollection(new Dictionary<string, StringValues> { ["random-question-id"] = "2", ["random-question"] = "1" });
                var result = RandomQuestionService.ValidateProvidedAnswer(dict);
            }
            catch
            {

            }
        }


        [Fact]
        public void Missing_Form_Should_Throw_Exception()
        {
            Assert.Throws<ArgumentNullException>(() => RandomQuestionService.ValidateProvidedAnswer(null));
        }

        [Fact]
        public void Empty_Form_Should_Throw_Exception()
        {
            var emptyForm = CreateEmptyForm();
            Assert.Throws<ArgumentNullException>(() => RandomQuestionService.ValidateProvidedAnswer(emptyForm));

            IFormCollection CreateEmptyForm() => new FormCollection(new Dictionary<string, StringValues>());
        }

        [Fact]
        public void Form_Missing_RandomQuestionField_Should_Throw_Exception()
        {
            var dict = CreateFormCollection(new Dictionary<string, StringValues> { ["email"] = "kmughal@gmail.com", ["random-question-id"] = "2" });
            Assert.Throws<InvalidOperationException>(() => RandomQuestionService.ValidateProvidedAnswer(dict));
        }

        [Fact]
        public void Form_Missing_RandomQuestionIdField_Should_Throw_Exception()
        {
            var dict = CreateFormCollection(new Dictionary<string, StringValues> { ["email"] = "kmughal@gmail.com", ["random-question"] = "2" });
            Assert.Throws<InvalidOperationException>(() => RandomQuestionService.ValidateProvidedAnswer(dict));
        }

        [Fact]
        public void Provided_Wrong_Answer_Should_Throw_Exception()
        {
            var dict = CreateFormCollection(new Dictionary<string, StringValues> { ["random-question-id"] = "2", ["random-question"] = "2" });
            Assert.Throws<InvalidOperationException>(() => RandomQuestionService.ValidateProvidedAnswer(dict));
        }

        [Fact]
        public void Provided_Correct_Answer_Should_BeOk()
        {
            var dict = CreateFormCollection(new Dictionary<string, StringValues> { ["random-question-id"] = "2", ["random-question"] = "1" });
            var result = RandomQuestionService.ValidateProvidedAnswer(dict);

            Assert.True(result);
        }


        public IFormCollection CreateFormCollection(Dictionary<string, StringValues> dict)
        {
            var form = new FormCollection(dict);
            return form;
        }
    }
}
