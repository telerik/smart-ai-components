using Telerik.Maui.SpeechRecognizer;

namespace SpeechToTextButtonWithOpenAIWhisper;

internal class WhisperSpeechRecognizer : IRadSpeechRecognizer
{
    private SpeechRecognizerState state;
    private IRecordAudio audioRecorder;
    private ITranscribeAudio audioTranscriber = null;

    public SpeechRecognizerState State
    {
        get => this.state;
        private set
        {
            if (this.state != value)
            {
                this.state = value;
                this.StateChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public event EventHandler StateChanged;
    public event EventHandler<SpeechRecognizerErrorOccurredEventArgs> ErrorOccurred;
    public event EventHandler<SpeechRecognizerSpeechRecognizedEventArgs> SpeechRecognized;

    public Task Init(SpeechRecognizerInitializationContext context)
    {
        if (this.State != SpeechRecognizerState.NotInitialized)
        {
            this.RaiseError("Can only initialize the recognizer if it is in a NotInitialized state.");
            return Task.CompletedTask;
        }

        this.State = SpeechRecognizerState.Ready;
        return Task.CompletedTask;
    }

    public async Task StartListening()
    {
        bool canStartListening = this.State == SpeechRecognizerState.Ready;

        if (!canStartListening)
        {
            this.RaiseError("You can only start listening if in the Initialized or NotListening states.");
            return;
        }

        this.audioRecorder = new MauiAudioRecorder();
        bool canRecordAudio = await this.audioRecorder.CanRecordAudio();

        if (canRecordAudio == false)
        {
            this.UnsetAudioRecorder();
            this.State = SpeechRecognizerState.Faulted;
            this.RaiseError("Cannot record audio.");
            return;
        }
        
        this.audioTranscriber = new WhisperAudioTranscriber();
        this.State = SpeechRecognizerState.StartingListening;

        try
        {
            await this.audioRecorder.StartAsync();
            this.State = SpeechRecognizerState.Listening;
        }
        catch (Exception exc)
        {
            this.UnsetAudioRecorder();
            this.State = SpeechRecognizerState.Faulted;
            this.RaiseError("Could not start recording.", exc);
            return;
        }
    }

    public async Task StopListening()
    {
        if (this.State != SpeechRecognizerState.Listening)
        {
            this.RaiseError("Can stop listening only if in Listening state.");
            return;
        }

        IRecordAudio localRecorder = this.audioRecorder;

        if (localRecorder == null)
        {
            this.State = SpeechRecognizerState.Ready;
            return;
        }

        this.State = SpeechRecognizerState.StoppingListening;
        this.UnsetAudioRecorder();
        Stream audioStream;

        try
        {
            audioStream = await localRecorder.StopAsync();
        }
        catch (Exception exc)
        {
            this.State = SpeechRecognizerState.Faulted;
            this.RaiseError("Error occured while stopping recording.", exc);
            return;
        }

        if (audioStream == null || audioStream.Length == 0)
        {
            audioStream?.Dispose();
            this.State = SpeechRecognizerState.Faulted;
            this.RaiseError("Audio stream is empty after stopping recording.");
            return;
        }

        try
        {
            string recognizedText = await this.audioTranscriber.TranscribeAsync(audioStream);
            this.RaiseSpeechRecognized(recognizedText);
        }
        catch (Exception exc)
        {
            this.State = SpeechRecognizerState.Faulted;
            this.RaiseError("Error occured while processing audio stream.", exc);
            return;
        }
        finally
        {
            audioStream.Dispose();
        }

        this.State = SpeechRecognizerState.Ready;
    }

    public ValueTask DisposeAsync()
    {
        this.State = SpeechRecognizerState.Disposing;
        this.audioRecorder?.StopAsync()?.Dispose();
        this.UnsetAudioRecorder();
        this.State = SpeechRecognizerState.Disposed;
        return new ValueTask(Task.CompletedTask);
    }

    public Task Reset()
    {
        this.audioRecorder?.StopAsync()?.Dispose();
        this.UnsetAudioRecorder();
        this.State = SpeechRecognizerState.NotInitialized;
        return Task.CompletedTask;
    }

    private void RaiseError(string message, Exception exc = null)
    {
        this.ErrorOccurred?.Invoke(this, new SpeechRecognizerErrorOccurredEventArgs(message, exc));
    }

    private void UnsetAudioRecorder()
    {
        if (this.audioRecorder != null)
        {
            this.audioRecorder = null;
        }
    }

    private void RaiseSpeechRecognized(string text)
    {
        this.SpeechRecognized?.Invoke(this, new SpeechRecognizerSpeechRecognizedEventArgs(text));
    }
}
