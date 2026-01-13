using System.Windows;
using Telerik.SpeechRecognizer;

namespace SpeechToTextButtonWithOpenAIWhisper;

public partial class MainWindow : Window
{
    private int originCursorPosition;
    private bool isInternalCursorPositionChange;
    private string lastFullText;
    private int cutOffTextLength;

    public MainWindow()
    {
        this.InitializeComponent();
        
        this.speechToTextButton.SpeechRecognizerCreator = () => new WhisperSpeechRecognizer();
    }

    private void SpeechToTextButton_StateChanged(object sender, EventArgs args)
    {
        if (this.speechToTextButton.State == SpeechRecognizerState.Listening)
        {
            this.originCursorPosition = this.textBox.CaretIndex;
            this.lastFullText = null;
            this.cutOffTextLength = -1;
        }
    }

    private void SpeechToTextButton_SpeechRecognized(object sender, SpeechRecognizerSpeechRecognizedEventArgs args)
    {
        string editorText = this.textBox.Text ?? string.Empty;
        int start = Math.Min(this.originCursorPosition, this.textBox.CaretIndex);
        int end = Math.Max(this.originCursorPosition, this.textBox.CaretIndex + this.textBox.SelectionLength);
        string fullText = args.FullText;

        if (this.cutOffTextLength > 0)
        {
            int cutOffStart = Math.Min(this.cutOffTextLength, args.FullText.Length);
            fullText = args.FullText.Substring(cutOffStart, args.FullText.Length - cutOffStart);
        }

        string newText = string.Concat(editorText.Substring(0, start), fullText, " ", editorText.Substring(end, editorText.Length - end));

        this.isInternalCursorPositionChange = true;
        this.textBox.Text = newText;
        this.textBox.CaretIndex = start + fullText.Length + 1;
        this.isInternalCursorPositionChange = false;

        this.lastFullText = args.FullText;
    }

    private void TextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
    {
        if (!this.isInternalCursorPositionChange)
        {
            this.originCursorPosition = this.textBox.CaretIndex;
            this.cutOffTextLength = this.lastFullText?.Length ?? -1;
        }
    }

    private void SpeechToTextButton_ErrorOccurred(object sender, SpeechRecognizerErrorOccurredEventArgs e)
    {
    }
}