namespace SpeechToTextButtonWithOpenAIWhisper;

internal interface IRecordAudio
{
    event EventHandler<AudioRecordedEventArgs> AudioRecorded;

    Task<bool> CanRecordAudio();
    Task StartAsync();
    Task<Stream> StopAsync();
}

internal class AudioRecordedEventArgs : EventArgs
{
    private Func<Stream> copyCurrentAudioStream;

    public AudioRecordedEventArgs(Func<Stream> copyCurrentAudioStream)
    {
        this.copyCurrentAudioStream = copyCurrentAudioStream;
    }

    public Stream CopyCurrentAudioStream()
    {
        return this?.copyCurrentAudioStream();
    }
}
