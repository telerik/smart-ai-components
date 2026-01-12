using Newtonsoft.Json.Linq;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;

namespace SpeechToTextButtonWithOpenAIWhisper;

internal class WhisperAudioTranscriber : ITranscribeAudio
{
    private const string ApiKey = "YOUR_API_KEY";

    private readonly object lockObject = new object();

    private HttpClient httpClient;

    public async Task<string> TranscribeAsync(Stream audioStream)
    {
        Task<string> transcriptionTask;

        lock (this.lockObject)
        {
            this.httpClient = this.httpClient ?? new HttpClient();
            transcriptionTask = this.GetTranscription(audioStream);
        }

        string transcription = await transcriptionTask;
        return transcription;
    }

    public void Dispose()
    {
        lock (this.lockObject)
        {
            this.httpClient?.Dispose();
            this.httpClient = null;
        }
    }

    private async Task<string> GetTranscription(Stream stream)
    {
        using (var form = new MultipartFormDataContent())
        using (var fileContent = new StreamContent(stream))
        using (var request = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/audio/transcriptions"))
        {
            fileContent.Headers.ContentType = new MediaTypeHeaderValue("audio/wav");
            string fileName = Guid.NewGuid().ToString("N") + ".wav";
            form.Add(fileContent, "file", fileName);
            form.Add(new StringContent("whisper-1"), "model");
            request.Content = form;
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", ApiKey);

            using (var response = await this.httpClient
                .SendAsync(request, HttpCompletionOption.ResponseContentRead, CancellationToken.None)
                .ConfigureAwait(false))
            {
                string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    var json = JObject.Parse(responseContent);
                    var textObject = json["text"];
                    string text = (string)textObject;
                    return text;
                }
                else
                {
                    string error = string.Format("Transcription failed ({0}): {1}", (int)response.StatusCode, responseContent);
                    return error;
                }
            }
        }
    }
}
