namespace RandomQuestion.Filter
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text.Json.Serialization;
    using System.Linq;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Primitives;

    public class RandomQuestion
    {
        [JsonPropertyName("question")]
        public string Question { set; get; }

        [JsonPropertyName("answer")]
        public string Answer { set; get; }

        [JsonPropertyName("id")]
        public string Id { set; get; }
    }

    public class RandomQuestions
    {
        private static string RANDOM_QUESTIONS_JSON_PATH => Path.Combine("data", "random-questions.json");

        private static string ReadQuestionsFromJson()
        {
            var contents = File.ReadAllText(RANDOM_QUESTIONS_JSON_PATH);
            return contents;
        }

        private static bool IsValid()
        {
            var result = File.Exists(RANDOM_QUESTIONS_JSON_PATH);
            if (!result)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(RANDOM_QUESTIONS_JSON_PATH));
                CreateJsonFileForQuestions();
                return false;
            }
            return result;
        }
        private static List<RandomQuestion> Deserialize(string contents)
        {
            var result = (List<RandomQuestion>)System.Text.Json.JsonSerializer.Deserialize<List<RandomQuestion>>(contents);
            return result;
        }

        private static void CreateJsonFileForQuestions()
        {
            var contents = @"[
                                {
                                    ""id"": ""1"",
                                    ""question"": ""2+2"",
                                    ""answer"": ""4""
                                },
                                {
                                    ""id"": ""2"",
                                    ""question"": ""13/13"",
                                    ""answer"": ""1""
                                },
                                {
                                    ""id"": ""3"",
                                    ""question"": ""What is the capital of UK?"",
                                    ""answer"": ""London""
                                }
                            ]";
            File.AppendAllText(RANDOM_QUESTIONS_JSON_PATH, contents);
        }

        public static List<RandomQuestion> CreateRandomQuestions()
        {
            if (!IsValid()) throw new FileNotFoundException("random-questions.json was not present, it has been created for you in the solution just update the contents and re-run the code!");
            var contents = ReadQuestionsFromJson();
            var result = Deserialize(contents);
            return result;
        }
    }

    public class RandomQuestionService
    {
        public static RandomQuestion GenerateRandomQuestion()
        {
            var qa = RandomQuestions.CreateRandomQuestions();
            var random = new Random();
            var index = random.Next(qa.Count);
            return qa[index];
        }

        public static bool ValidateProvidedAnswer(IFormCollection form)
        {
            ValidateForm(form);
            var (providedAnswer, questionId) = ExtractAnswerFromRequest(form);

            var qa = RandomQuestions.CreateRandomQuestions();

            var lookup = qa.FirstOrDefault(q => q.Id == questionId);
            if (lookup is null) return false;

            var result = string.Equals(lookup.Answer, providedAnswer, StringComparison.OrdinalIgnoreCase);
            return result ? true : throw new InvalidOperationException("Provided answer is not correct!");
        }

        private static (string providedAnswer, string questionId) ExtractAnswerFromRequest(IFormCollection form)
        {
            var (randomQuestionPresent, providedAnswer, questionIdPresent, questionId) = RandomQuestionPresent(form);
            if (!randomQuestionPresent || !questionIdPresent) throw new InvalidOperationException("Please provide the answer!");
            return (providedAnswer, questionId);
        }

        private static (bool randomQuestionPresent, Microsoft.Extensions.Primitives.StringValues providedAnswer, bool questionIdPresent, string questionId) RandomQuestionPresent(IFormCollection form)
        {
            var randomQuestionPresent = form.TryGetValue("random-question", out StringValues providedAnswer);
            var questionIdPresent = form.TryGetValue("random-question-id", out StringValues questionId);

            return (randomQuestionPresent, providedAnswer, questionIdPresent, questionId);
        }

        private static void ValidateForm(IFormCollection form)
        {
            if (form is null || form.Keys is null || !form.Keys.Any())
                throw new ArgumentNullException("FormCollection is empty!");
        }
    }
}
