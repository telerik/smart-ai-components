using Azure;
using Azure.AI.OpenAI;
using SmartComponents.LocalEmbeddings;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using Telerik.Windows.Controls;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Docx;
using Telerik.Windows.Documents.Model;
using Telerik.Windows.Documents.RichTextBoxCommands;

namespace SmartRichTextBox
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Dictionary<string, EmbeddingF32> PageEmbeddings { get; set; }
        public string AllTextContent { get; set; }

        static MainWindow()
        {
            StyleManager.ApplicationTheme = new Windows11Theme();
            RadRibbonWindow.IsWindowsThemeEnabled = !false;
        }

        public MainWindow()
        {
            InitializeComponent();
            IconSources.ChangeIconsSet(IconsSet.Light);
            DocxFormatProvider provider = new DocxFormatProvider();
            radRichTextBox.Document = provider.Import(File.ReadAllBytes(@"..\..\..\SampleData\New App specification.docx"));

            radRichTextBox.PreviewKeyDown += RadRichTextBox_PreviewKeyDown;

            radRichTextBox.CommandExecuted += RadRichTextBox_CommandExecuted;

            timer.Interval = TimeSpan.FromSeconds(2);
            timer.Tick += Timer_Tick;

            AllTextContent = File.ReadAllText(@"..\..\..\Context.txt");
            string[] chunks = AllTextContent.Split("--- NEW PAGE ---", StringSplitOptions.RemoveEmptyEntries);

            LocalEmbedder embedder = new LocalEmbedder();
            PageEmbeddings = chunks.Select(x => KeyValuePair.Create(x, embedder.Embed(x))).ToDictionary(k => k.Key, v => v.Value);

        }

        private readonly DispatcherTimer timer = new DispatcherTimer();
        private bool textCommitted = true;

        private void Timer_Tick(object sender, EventArgs e)
        {
            string question = GetCurrentText();

            if(string.IsNullOrWhiteSpace(question))
            {
                return;
            }

            string answer = AnswerQuestion(question);
            Debug.WriteLine(question);
            Debug.WriteLine(answer);
            AppendText(radRichTextBox, answer);

            timer.Stop();
            suppressTextChanges = false;
        }

        private string GetCurrentText()
        {
            Paragraph paragraph = radRichTextBox.Document.CaretPosition.GetCurrentParagraph();
            StringBuilder sb = new StringBuilder();

            if(paragraph != null)
            {
                foreach(Span span in paragraph.EnumerateChildrenOfType<Span>())
                {
                    sb.Append(span.Text);
                }
            }
            return sb.ToString();
        }
        private void RadRichTextBox_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if(e.Key == System.Windows.Input.Key.Tab)
            { 
                Paragraph p = radRichTextBox.Document.CaretPosition.GetCurrentParagraph();
                Span span = p.Inlines.OfType<Span>().Where(s => s.ForeColor == Colors.LightGray).LastOrDefault();

                if(span != null)
                {
                    e.Handled = true;

                    span.ForeColor = Colors.Black;
                    radRichTextBox.UpdateEditorLayout();
                    radRichTextBox.Document.CaretPosition.MoveToDocumentElementEnd(p);

                    textCommitted = true;
                }
            }
        }

        private bool suppressTextChanges;
        private void RadRichTextBox_CommandExecuted(object sender, CommandExecutedEventArgs e)
        {
            if(e.Command is InsertTextCommand)
            {
                timer.Stop();

                if(textCommitted)
                {
                    timer.Start();
                }
            }
        }

        public void AppendText(RadRichTextBox box, string text)
        {
            Span span = new Span(text)
            {
                ForeColor = Colors.LightGray
            };

            box.InsertInline(span);

            radRichTextBox.Document.CaretPosition.MoveToDocumentElementStart(span);
        }

        private string AnswerQuestion(string question)
        {
            LocalEmbedder embedder = new LocalEmbedder();
            EmbeddingF32 questionEmbedding = embedder.Embed(question);

            string[] results = LocalEmbedder.FindClosest(questionEmbedding, PageEmbeddings.Select(x => (x.Key, x.Value)), 2);

            string answer = CallOpenAIApi("You are a helpful assistant. Use the provided context to answer the user question. Context: " + string.Join(" --- ", results), question);

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

