using Azure;
using Azure.AI.OpenAI;
using SmartComponents.LocalEmbeddings;
using System.Text;
using Telerik.WinControls.UI;
using Telerik.WinForms.Documents.FormatProviders.OpenXml.Docx;
using Telerik.WinForms.Documents.Model;
using Telerik.WinForms.Documents.RichTextBoxCommands;
using Timer = System.Windows.Forms.Timer;

namespace SmartAIComponents
{
    public partial class MainForm : RadForm
    {
        private Timer timer;

        public MainForm()
        {
            InitializeComponent();

            DocxFormatProvider provider = new DocxFormatProvider();
            this.radRichTextSmartEditor.Document = provider.Import(File.ReadAllBytes(@"..\..\..\SampleData\New_App_specification.docx"));

            this.radRichTextSmartEditor.RichTextBoxElement.PreviewEditorKeyDown += this.RichTextBoxElement_PreviewEditorKeyDown;
            this.radRichTextSmartEditor.CommandExecuted += this.RadRichTextEditor_CommandExecuted;

            this.timer = new Timer();
            this.timer.Interval = 2000;
            this.timer.Tick += Timer_Tick;

            AllTextContent = File.ReadAllText(@"..\..\..\Context.txt");
            string[] chunks = AllTextContent.Split("--- NEW PAGE ---", StringSplitOptions.RemoveEmptyEntries);

            LocalEmbedder embedder = new LocalEmbedder();
            PageEmbeddings = chunks.Select(x => KeyValuePair.Create(x, embedder.Embed(x))).ToDictionary(k => k.Key, v => v.Value);
        }

        public Dictionary<string, EmbeddingF32> PageEmbeddings { get; set; }

        public string AllTextContent { get; set; }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            timer.Stop();

            string question = this.GetCurrentText();
            if (string.IsNullOrEmpty(question))
            {
                return;
            }

            string answer = this.AnswerQuestion(question);
            AppendText(this.radRichTextSmartEditor, answer);
        }

        private string GetCurrentText()
        {
            Paragraph paragraph = this.radRichTextSmartEditor.Document.CaretPosition.GetCurrentParagraph();
            StringBuilder sb = new StringBuilder();

            if (paragraph != null)
            {
                foreach (Span span in paragraph.EnumerateChildrenOfType<Span>())
                {
                    sb.Append(span.Text);
                }
            }

            return sb.ToString();
        }

        private string AnswerQuestion(string question)
        {
            LocalEmbedder embedder = new LocalEmbedder();
            EmbeddingF32 questionEmbedding = embedder.Embed(question);

            string[] results = LocalEmbedder.FindClosest(questionEmbedding, PageEmbeddings.Select(x => (x.Key, x.Value)), 2);

            string answer = CallOpenAIApi("You are a helpful assistant. Use the provided context to answer the user question. Context: " + string.Join(" --- ", results), question);

            return answer;
        }

        private static string CallOpenAIApi(string systemPrompt, string message)
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

        private static void AppendText(RadRichTextEditor editor, string text)
        {
            Span s = new Span(text);
            s.ForeColor = Color.DimGray;

            editor.InsertInline(s);
            editor.Document.CaretPosition.MoveToDocumentElementStart(s);
        }

        private void RadRichTextEditor_CommandExecuted(object? sender, CommandExecutedEventArgs e)
        {
            if (e.Command is InsertTextCommand)
            {
                this.timer.Stop();
                this.timer.Start();
            }
        }

        private void RichTextBoxElement_PreviewEditorKeyDown(object sender, Telerik.WinForms.Documents.PreviewEditorKeyEventArgs e)
        {
            if (e.OriginalArgs.KeyCode == Keys.Tab)
            {
                var p = this.radRichTextSmartEditor.Document.CaretPosition.GetCurrentParagraph();
                var span = p.Inlines.OfType<Span>().Where(s => Color.DimGray == s.ForeColor).LastOrDefault();
                if (span != null)
                {
                    e.SuppressDefaultAction = true;

                    span.ForeColor = Color.Black;
                    this.radRichTextSmartEditor.UpdateEditorLayout();
                    this.radRichTextSmartEditor.Document.CaretPosition.MoveToDocumentElementEnd(p);
                }
            }
        }
    }
}
