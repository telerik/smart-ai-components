using OpenAI;
using OpenAI.Audio;

namespace SpeechToTextButtonWithOpenAIWhisper;

internal class WhisperAudioTranscriber : ITranscribeAudio
{
    private readonly OpenAIClient openAiClient;

    public WhisperAudioTranscriber()
    {
        string apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");

        if (string.IsNullOrWhiteSpace(apiKey))
        {
            throw new InvalidOperationException("Missing OPENAI_API_KEY environment variable.");
        }

        this.openAiClient = new OpenAIClient(apiKey);
    }

    public async Task<string> TranscribeAsync(Stream audioStream)
    {
        AudioClient audioClient = this.openAiClient.GetAudioClient(model: "whisper-1");

        AudioTranscriptionOptions options = new AudioTranscriptionOptions
        {
            ResponseFormat = AudioTranscriptionFormat.Text,
        };

        AudioTranscription transcription = await audioClient
            .TranscribeAudioAsync(audioStream, audioFilename: $"{Guid.NewGuid():N}.wav", options, CancellationToken.None)
            .ConfigureAwait(false);

        return transcription.Text;
    }
}
