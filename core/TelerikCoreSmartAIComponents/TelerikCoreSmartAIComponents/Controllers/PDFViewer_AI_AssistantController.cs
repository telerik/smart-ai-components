using Azure.AI.OpenAI;
using Azure;
using Microsoft.AspNetCore.Mvc;
using SmartComponents.LocalEmbeddings;

namespace TelerikCoreSmartAIComponents.Controllers
{
    public class PDFViewer_AI_AssistantController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GenerateSuggestions(string text)
        {
            var questionsJson = await CallOpenAIApi(
                    @"You are a helpful assistant. Your task is to analyze the provided text and generate 3 short diverse questions.
            The qustions should be returned in form of a string array in a valid JSON format. Return only the JSON and nothing else.",
            text);
            var suggestions = System.Text.Json.JsonSerializer.Deserialize<List<string>>(questionsJson.Replace("```json", "").Replace("```", ""));

            return Json(suggestions);
        }

        public async Task<IActionResult> AnswerQuestion(string text, string question)
        {
            var questionEmbedding = EmbedText(question);
            var contentEmbeddings = EmbedContent(text);
            var results = LocalEmbedder.FindClosest(questionEmbedding, contentEmbeddings.Select(x => (x.Key, x.Value)), 2);
            var answer = await CallOpenAIApi("You are a helpful assistant. Use the provided context to answer the user question. Context: " + string.Join(" --- ", results), question);

            return Json(new { answer = answer });
        }

        private Dictionary<string, EmbeddingF32> EmbedContent(string text)
        {
            var chunks = text.Split("--- NEW PAGE ---", StringSplitOptions.RemoveEmptyEntries);

            var embeddings = chunks.Select(x => KeyValuePair.Create(x, EmbedText(x))).ToDictionary(k => k.Key, v => v.Value);

            return embeddings;
        }

        private EmbeddingF32 EmbedText(string text)
        {
            var embedder = new LocalEmbedder();

            return embedder.Embed(text);
        }

        private async Task<string> CallOpenAIApi(string systemPrompt, string message)
        {
            var client = new OpenAIClient(
             new Uri("AZURE_ENDPOINT"),
             new AzureKeyCredential("PUT_YOUR_API_KEY_HERE"));

            var chatCompletionsOptions = new ChatCompletionsOptions()
            {
                DeploymentName = "DEPLOYMENT_NAME",
                Messages =
             {
                 new ChatRequestSystemMessage(systemPrompt),
                 new ChatRequestUserMessage(message),
             }
            };

            var response = await client.GetChatCompletionsAsync(chatCompletionsOptions);
            var responseMessage = response.Value.Choices[0].Message;

            return responseMessage.Content;
        }
    }
}
