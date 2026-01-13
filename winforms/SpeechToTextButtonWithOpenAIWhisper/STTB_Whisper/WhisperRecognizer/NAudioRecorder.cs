using NAudio.Wave;

namespace SpeechToTextButtonWithOpenAIWhisper;

internal class NAudioRecorder : IRecordAudio
{
    private readonly object lockObject = new object();

    private WaveInEvent waveIn;
    private WaveFileWriter waveWriter;
    private MemoryStream audioStream;

    public event EventHandler<AudioRecordedEventArgs> AudioRecorded;

    public Task<bool> CanRecordAudio()
    {
        return Task.FromResult(true);
    }

    public Task StartAsync()
    {
        return Task.Run(() =>
        {
            lock (this.lockObject)
            {
                MemoryStream localAudioStream = new MemoryStream();
                WaveFormat waveFormat = new WaveFormat(16000, 1);
                WaveFileWriter localWaveWriter = new WaveFileWriter(localAudioStream, waveFormat);
                WaveInEvent localWaveIn = new WaveInEvent { WaveFormat = waveFormat, BufferMilliseconds = 100 };
                localWaveIn.DataAvailable += this.OnWaveInDataAvailable;
                localWaveIn.StartRecording();

                this.audioStream = localAudioStream;
                this.waveWriter = localWaveWriter;
                this.waveIn = localWaveIn;
            }
        });
    }

    public Task<Stream> StopAsync()
    {
        return Task.Run<Stream>(() =>
        {
            lock (this.lockObject)
            {
                if (this.waveIn != null)
                {
                    try
                    {
                        WaveInEvent localWaveIn = this.waveIn;
                        this.waveIn = null;
                        localWaveIn.DataAvailable -= this.OnWaveInDataAvailable;
                        localWaveIn.StopRecording();
                        localWaveIn.Dispose();
                    }
                    catch
                    {
                    }
                }

                if (this.waveWriter != null)
                {
                    try
                    {
                        WaveFileWriter localWaveWriter = this.waveWriter;
                        this.waveWriter = null;
                        localWaveWriter.Flush();
                    }
                    catch
                    {
                    }
                }

                MemoryStream localMemoryStream = this.audioStream;
                localMemoryStream.Position = 0;
                this.audioStream = null;
                return localMemoryStream;
            }
        });
    }

    private void OnWaveInDataAvailable(object sender, WaveInEventArgs e)
    {
        lock (this.lockObject)
        {
            if (sender == this.waveIn)
            {
                this.waveWriter.Write(e.Buffer, 0, e.BytesRecorded);
                this.waveWriter.Flush();
                this.AudioRecorded?.Invoke(this, new AudioRecordedEventArgs(this.CopyCurrentAudioStream));
            }
        }
    }

    private Stream CopyCurrentAudioStream()
    {
        long savePosition = this.audioStream.Position;
        this.audioStream.Position = 0;
        MemoryStream copyStream = new MemoryStream();
        this.audioStream.CopyTo(copyStream);
        this.audioStream.Position = savePosition;
        copyStream.Position = 0;
        return copyStream;
    }
}
