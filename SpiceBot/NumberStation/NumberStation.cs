using System;
using System.IO;
using System.Threading.Tasks;
using System.Timers;
using Discord.Audio;

namespace SpiceBot
{
    internal class NumberStation : IDisposable
    {
        private readonly IAudioClient _audioClient;
        private readonly AudioOutStream _pcmStream;
        private readonly Timer _numberPlayTimer;

        public NumberStation(IAudioClient audioClient)
        {
            _audioClient = audioClient;
            _audioClient.Disconnected += AudioClientOnDisconnected;
            _pcmStream = audioClient.CreateDirectPCMStream(AudioApplication.Voice);
            _numberPlayTimer = new Timer(2000);
            _numberPlayTimer.Elapsed += NumberPlayTimerOnElapsed;
            _numberPlayTimer.Start();
        }

        private async void NumberPlayTimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            _numberPlayTimer.Stop();
            await _audioClient.SetSpeakingAsync(true);
            await using var bytes = File.OpenRead(@"16bitPCMFile.raw");
            await bytes.CopyToAsync(_pcmStream);
            await _audioClient.SetSpeakingAsync(false);
            _numberPlayTimer.Start();
        }

        private Task AudioClientOnDisconnected(Exception arg)
        {
            _numberPlayTimer.Stop();
            Dispose();
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _audioClient.Dispose();
            _pcmStream.Dispose();
            _numberPlayTimer.Dispose();
        }
    }
}