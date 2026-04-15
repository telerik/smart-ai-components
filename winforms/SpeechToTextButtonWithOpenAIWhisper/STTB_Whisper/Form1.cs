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
        this.textBoxControl.Multiline = true;
        this.textBoxControl.WordWrap = true;
    }

    private void SpeechToTextButton_StateChanged(object sender, EventArgs args)
    {
        if (this.speechToTextButton.State == SpeechRecognizerState.Listening)
        {
            this.originCursorPosition = this.textBoxControl.SelectionStart;
            this.lastFullText = null;
            this.cutOffTextLength = -1;
        }
    }

    private void SpeechToTextButton_SpeechRecognized(object sender, SpeechRecognizerSpeechRecognizedEventArgs args)
    {
        string editorText = this.textBoxControl.Text ?? string.Empty;
        int start = Math.Min(this.originCursorPosition, this.textBoxControl.SelectionStart);
        int end = Math.Max(this.originCursorPosition, this.textBoxControl.SelectionStart + this.textBoxControl.SelectionLength);
        string fullText = args.FullText;

        if (this.cutOffTextLength > 0)
        {
            int cutOffStart = Math.Min(this.cutOffTextLength, args.FullText.Length);
            fullText = args.FullText.Substring(cutOffStart, args.FullText.Length - cutOffStart);
        }

        string newText = string.Concat(editorText.Substring(0, start), fullText, " ", editorText.Substring(end, editorText.Length - end));

        this.isInternalCursorPositionChange = true;
        this.textBoxControl.Text = newText;
        this.textBoxControl.SelectionStart = start + fullText.Length + 1;
        this.isInternalCursorPositionChange = false;

        this.lastFullText = args.FullText;
    }

    private void TextBoxControl_SelectionChanged(object sender, Telerik.WinControls.UI.SelectionChangedEventArgs e)
    {
        if (!this.isInternalCursorPositionChange)
        {
            this.originCursorPosition = this.textBoxControl.SelectionStart;
            this.cutOffTextLength = this.lastFullText?.Length ?? -1;
        }
    }

    private void SpeechToTextButton_ErrorOccurred(object sender, SpeechRecognizerErrorOccurredEventArgs e)
    {
    }
}
