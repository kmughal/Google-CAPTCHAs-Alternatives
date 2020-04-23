using System.Linq;
namespace Web.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text.Json;
    using System.Text.Json.Serialization;

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

        private static string ReadQuestionsFromJson()
        {
            var fullPath = Path.Combine("data", "random-questions.json");
            var contents = File.ReadAllText(fullPath);
            return contents;
        }


        private static List<RandomQuestion> Deserialize(string contents)
        {
            var result = (List<RandomQuestion>)System.Text.Json.JsonSerializer.Deserialize<List<RandomQuestion>>(contents);
            return result;
        }

        public static List<RandomQuestion> CreateRandomQuestions()
        {
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

        public static bool ValidateProvidedAnswer(string questionId, string providedAnswer)
        {
            var qa = RandomQuestions.CreateRandomQuestions();

            var lookup = qa.FirstOrDefault(q => q.Id == questionId);
            if (lookup is null) return false;

            var result = string.Equals(lookup.Answer, providedAnswer, StringComparison.OrdinalIgnoreCase);
            return result;
        }

    }
}