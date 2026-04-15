namespace SpeechToTextButtonWithOpenAIWhisper;

internal interface IRecordAudio
{
    Task<bool> CanRecordAudio();
    Task StartAsync();
    Task<Stream> StopAsync();
}
