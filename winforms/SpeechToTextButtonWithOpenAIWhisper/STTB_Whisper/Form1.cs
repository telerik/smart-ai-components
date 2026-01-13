using Telerik.SpeechRecognizer;

namespace SpeechToTextButtonWithOpenAIWhisper;

public partial class Form1 : Form
{
    private int originCursorPosition;
    private bool isInternalCursorPositionChange;
    private string lastFullText;
    private int cutOffTextLength;

    public Form1()
    {
        this.InitializeComponent();

        this.speechToTextButton.SpeechRecognizerCreator = () => new WhisperSpeechRecognizer();
    }

    private void SpeechToTextButton_StateChanged(object sender, EventArgs args)
    {
        if (this.speechToTextButton.State == SpeechRecognizerState.Listening)
        {
            this.originCursorPosition = this.textBox1.SelectionStart;
            this.lastFullText = null;
            this.cutOffTextLength = -1;
        }
    }

    private void SpeechToTextButton_SpeechRecognized(object sender, SpeechRecognizerSpeechRecognizedEventArgs args)
    {
        string editorText = this.textBox1.Text ?? string.Empty;
        int start = Math.Min(this.originCursorPosition, this.textBox1.SelectionStart);
        int end = Math.Max(this.originCursorPosition, this.textBox1.SelectionStart + this.textBox1.SelectionLength);
        string fullText = args.FullText;

        if (this.cutOffTextLength > 0)
        {
            int cutOffStart = Math.Min(this.cutOffTextLength, args.FullText.Length);
            fullText = args.FullText.Substring(cutOffStart, args.FullText.Length - cutOffStart);
        }

        string newText = string.Concat(editorText.Substring(0, start), fullText, " ", editorText.Substring(end, editorText.Length - end));

        this.isInternalCursorPositionChange = true;
        this.textBox1.Text = newText;
        this.textBox1.SelectionStart = start + fullText.Length + 1;
        this.isInternalCursorPositionChange = false;

        this.lastFullText = args.FullText;
    }

    private void TextBox1_TextChanged(object sender, EventArgs e)
    {
        if (!this.isInternalCursorPositionChange)
        {
            this.originCursorPosition = this.textBox1.SelectionStart;
            this.cutOffTextLength = this.lastFullText?.Length ?? -1;
        }
    }

    private void SpeechToTextButton_ErrorOccurred(object sender, SpeechRecognizerErrorOccurredEventArgs e)
    {
    }
}
