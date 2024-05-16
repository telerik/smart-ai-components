using Azure;
using Azure.AI.OpenAI;
using SmartComponents.LocalEmbeddings;
using Telerik.Windows.Documents.Spreadsheet.Expressions;
using Telerik.Windows.Documents.Spreadsheet.Expressions.Functions;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx;

namespace SmartSpreadsheet
{
    public partial class MainForm : Telerik.WinControls.UI.RadForm
    {
        public MainForm()
        {
            InitializeComponent();

            FunctionManager.RegisterFunction(new AIFunction());
            XlsxFormatProvider provider = new XlsxFormatProvider();
            this.radSpreadsheetSmart.Workbook = provider.Import(File.ReadAllBytes(@"..\..\..\SampleData\AI_Template.xlsx"));
        }

        public class AIFunction : StringsInFunction
        {
            public Dictionary<string, EmbeddingF32> PageEmbeddings { get; set; }

            public string AllTextContent { get; set; }

            ///<summary>
            /// The name of the function.
            ///</summary>
            public static readonly string FunctionName = "AI";

            private static readonly FunctionInfo Info;

            /// <summary>
            /// Gets the name of the function.
            /// </summary>
            /// <returns>The name as String.</returns>
            /// <value>The name.</value>
            public override string Name
            {
                get
                {
                    return FunctionName;
                }
            }

            /// <summary>
            /// Gets the function info.
            /// </summary>
            /// <returns>The function info as FunctionInfo.</returns>
            /// <value>The function info.</value>
            public override FunctionInfo FunctionInfo
            {
                get
                {
                    return Info;
                }
            }

            public AIFunction()
            {

                AllTextContent = File.ReadAllText(@"..\..\..\Context.txt");
                string[] chunks = AllTextContent.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

                LocalEmbedder embedder = new LocalEmbedder();
                PageEmbeddings = chunks.Select(x => KeyValuePair.Create(x, embedder.Embed(x))).ToDictionary(k => k.Key, v => v.Value);
            }

            static AIFunction()
            {
                string description = "Returns the result of an AI prompt";
                string descriptionKey = "Spreadsheet_Functions_Code_Info";

                IEnumerable<ArgumentInfo> requiredArguments = new ArgumentInfo[]
                {
                    new ArgumentInfo("Prompt",
                                    "The AI prompt you want to execute.",
                                    ArgumentType.Text)
                };

                Info = new FunctionInfo(FunctionName, FunctionCategory.Text, description, requiredArguments, descriptionLocalizationKey: descriptionKey);
            }

            /// <summary>
            /// Virtual method evaluating the function with System.Object arguments array.
            /// </summary>
            /// <param name="context">The context.</param>
            /// <returns>Functions result as RadExpression.</returns>
            protected override RadExpression EvaluateOverride(FunctionEvaluationContext<string> context)
            {
                string text = context.Arguments[0];

                if (text.Length == 0)
                {
                    return ErrorExpressions.ValueError;
                }

                Task<string> promptTask = GetResultFromAIPrompt(text);
                promptTask.Wait();

                string response = promptTask.Result;

                double value;
                if (double.TryParse(response, out value))
                {
                    return new NumberExpression(value);
                }

                return new StringExpression(response);
            }

            private Task<string> GetResultFromAIPrompt(string text)
            {
                return Task.Factory.StartNew(() =>
                {
                    string result = AnswerQuestion(text);
                    return result;
                });
            }

            private string AnswerQuestion(string question)
            {
                LocalEmbedder embedder = new LocalEmbedder();
                EmbeddingF32 questionEmbedding = embedder.Embed(question);

                string[] results = LocalEmbedder.FindClosest(questionEmbedding, PageEmbeddings.Select(x => (x.Key, x.Value)), 2);

                string answer = CallOpenAIApi("Answer the following questions about Formula 1 one: " + string.Join(" --- ", results), question);

                return answer;
            }

            private string CallOpenAIApi(string systemPrompt, string message)
            {
                // Add your key and endpoint  to use the OpenAI API.
                OpenAIClient client = new OpenAIClient(
                    new Uri("AZURE_ENDPOINT"),
                    new AzureKeyCredential("AZURE_KEY"));

                ChatCompletionsOptions chatCompletionsOptions = new ChatCompletionsOptions()
                {
                    DeploymentName = "DeploymentName",
                    Messages =
                {
                    new ChatRequestSystemMessage(systemPrompt),
                    new ChatRequestUserMessage(message),
                }
                };

                Response<ChatCompletions> response = client.GetChatCompletions(chatCompletionsOptions);
                ChatResponseMessage responseMessage = response.Value.Choices[0].Message;

                return responseMessage.Content;
            }
        }
    }
}
