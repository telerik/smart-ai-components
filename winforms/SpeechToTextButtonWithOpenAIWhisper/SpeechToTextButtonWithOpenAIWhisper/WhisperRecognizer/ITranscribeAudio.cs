namespace SpeechToTextButtonWithOpenAIWhisper;

internal interface ITranscribeAudio
{
    Task<string> TranscribeAsync(Stream audioStream);
    void Dispose();
}
