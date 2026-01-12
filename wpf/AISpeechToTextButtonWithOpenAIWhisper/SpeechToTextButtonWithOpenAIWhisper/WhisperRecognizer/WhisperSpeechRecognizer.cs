using System.IO;
using Telerik.SpeechRecognizer;

namespace SpeechToTextButtonWithOpenAIWhisper;

internal class WhisperSpeechRecognizer : IRadSpeechRecognizer
{
    private SpeechRecognizerState state;
    private SpeechRecognizerInitializationContext initContext;
    private IRecordAudio audioRecorder;
    private DateTime lastTimeAudioSentForTranscription;
    private DateTime lastTimeTranscriptionReceived;
    private ITranscribeAudio audioTranscriber = null;
    private string lastReportedText;

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
        this.initContext = context;

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

        this.audioRecorder = new NAudioRecorder();
        this.audioRecorder.AudioRecorded += this.AudioRecorder_AudioRecorded;
        this.lastReportedText = string.Empty;
        this.lastTimeAudioSentForTranscription = DateTime.Now;
        this.lastTimeTranscriptionReceived = DateTime.Now;
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
            this.audioRecorder.AudioRecorded -= this.AudioRecorder_AudioRecorded;
            this.audioRecorder = null;
        }
    }

    private void AudioRecorder_AudioRecorded(object sender, AudioRecordedEventArgs args)
    {
        if (!this.initContext.IsContinuousRecognition)
        {
            return;
        }

        // Important note:
        // OpenAI whisper-1 does not support live transcribe natively, so we send the audio every 0.5 seconds to get some feeling of real-time transcription.
        // Keep in mind that this will increase the used minutes.

        DateTime now = DateTime.Now;
        TimeSpan elapsed = now - this.lastTimeAudioSentForTranscription;

        if (0.5 < elapsed.Seconds)
        {
            this.lastTimeAudioSentForTranscription = now;
            DateTime timeOfSending = now;
            Stream audioStream = args.CopyCurrentAudioStream();
            ITranscribeAudio localAudioTranscriber = this.audioTranscriber;

            localAudioTranscriber?.TranscribeAsync(audioStream).ContinueWith(t =>
            {
                if (!t.IsFaulted && this.audioTranscriber == localAudioTranscriber && this.lastTimeTranscriptionReceived < timeOfSending)
                {
                    this.lastTimeTranscriptionReceived = timeOfSending;
                    this.RaiseSpeechRecognized(t.Result);
                }

                audioStream.Dispose();
            });
        }
    }

    private void RaiseSpeechRecognized(string text)
    {
        var h = this.SpeechRecognized;

        if (h != null && this.lastReportedText != text)
        {
            this.lastReportedText = text;
            h.Invoke(this, new SpeechRecognizerSpeechRecognizedEventArgs(text));
        }
    }
}
