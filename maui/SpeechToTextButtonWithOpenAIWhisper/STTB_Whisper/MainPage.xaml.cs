using Telerik.Maui.SpeechRecognizer;

namespace SpeechToTextButtonWithOpenAIWhisper;

public partial class MainPage : ContentPage
{
    private int originCursorPosition;
    private bool isInternalCursorPositionChange;
    private string lastFullText;
    private int cutOffTextLength;

    public MainPage()
    {
        this.InitializeComponent();

        this.speechToTextButton.SpeechRecognizerCreator = () => new WhisperSpeechRecognizer();
    }

    private void SpeechToTextButton_StateChanged(object sender, EventArgs args)
    {
        if (this.speechToTextButton.State == SpeechRecognizerState.Listening)
        {
            this.originCursorPosition = this.editor.CursorPosition;
            this.lastFullText = null;
            this.cutOffTextLength = -1;
        }
    }

    private void SpeechToTextButton_SpeechRecognized(object sender, SpeechRecognizerSpeechRecognizedEventArgs args)
    {
        string editorText = this.editor.Text ?? string.Empty;
        int start = Math.Min(this.originCursorPosition, this.editor.CursorPosition);
        int end = Math.Max(this.originCursorPosition, this.editor.CursorPosition + this.editor.SelectionLength);
        string fullText = args.FullText;

        if (this.cutOffTextLength > 0)
        {
            int cutOffStart = Math.Min(this.cutOffTextLength, args.FullText.Length);
            fullText = args.FullText.Substring(cutOffStart, args.FullText.Length - cutOffStart);
        }

        string newText = string.Concat(editorText.Substring(0, start), fullText, " ", editorText.Substring(end, editorText.Length - end));

        this.isInternalCursorPositionChange = true;
        this.editor.Text = newText;
        this.editor.CursorPosition = start + fullText.Length + 1;
        this.isInternalCursorPositionChange = false;

        this.lastFullText = args.FullText;
    }

    private void Editor_TextChanged(object sender, TextChangedEventArgs args)
    {
        if (!this.isInternalCursorPositionChange)
        {
            this.originCursorPosition = this.editor.CursorPosition;
            this.cutOffTextLength = this.lastFullText?.Length ?? -1;
        }
    }

    private void SpeechToTextButton_ErrorOccurred(object sender, SpeechRecognizerErrorOccurredEventArgs args)
    {
    }
}