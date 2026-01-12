using Plugin.Maui.Audio;

namespace SpeechToTextButtonWithOpenAIWhisper;

internal class MauiAudioRecorder : IRecordAudio
{
    private readonly object lockObject = new object();

    private IAudioManager audioManager;
    private IAudioRecorder recorder;

    public async Task<bool> CanRecordAudio()
    {
        bool hasMicPermissions = await TryGetMicPermissions();

        if (!hasMicPermissions)
        {
            return false;
        }

        if (this.recorder == null)
        {
            this.audioManager ??= new AudioManager();
            this.recorder = this.audioManager.CreateRecorder();
        }

        return this.recorder.CanRecordAudio;
    }

    public async Task StartAsync()
    {
        Task startRecordTast;

        lock (this.lockObject)
        {
            this.audioManager ??= new AudioManager();
            this.recorder ??= this.audioManager.CreateRecorder();
            startRecordTast = this.recorder.StartAsync();
        }

        await startRecordTast;
    }

    public async Task<Stream> StopAsync()
    {
        IAudioRecorder localRecorder;

        lock (this.lockObject)
        {
            localRecorder = this.recorder;

            if (localRecorder == null)
            {
                return null;
            }

            this.recorder = null;
        }

        try
        {
            IAudioSource audioSource = await localRecorder.StopAsync();
            Stream audioStream = audioSource.GetAudioStream();
            audioStream.Position = 0;
            return audioStream;
        }
        catch
        {
            return null;
        }
    }

    private static async Task<bool> TryGetMicPermissions()
    {
        PermissionStatus micPermissions = await Permissions.CheckStatusAsync<Permissions.Microphone>();

        if (micPermissions == PermissionStatus.Granted)
        {
            return true;
        }

        micPermissions = await Permissions.RequestAsync<Permissions.Microphone>();

        if (micPermissions == PermissionStatus.Granted)
        {
            return true;
        }

        return false;
    }
}