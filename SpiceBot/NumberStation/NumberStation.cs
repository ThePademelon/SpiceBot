using System;
using System.IO;
using System.Threading.Tasks;
using System.Timers;
using Discord.Audio;

namespace SpiceBot.NumberStation
{
    internal class NumberStation : IDisposable
    {
        private readonly IAudioClient _audioClient;
        private readonly AudioOutStream _pcmStream;
        private readonly Timer _numberPlayTimer;
        private readonly NumberGenerator _numberGenerator;

        public NumberStation(IAudioClient audioClient)
        {
            _numberGenerator = new NumberGenerator();
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
            var bytes = _numberGenerator.GetNumber();
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

    internal class NumberGenerator
    {
        private readonly Random _random;

        /// <summary>
        /// The location of the audio files on my personal computer.
        /// Will replace with deployment friendly assembly resource/folder once I'm done debugging.
        /// </summary>
        private const string CodebaseFileLocation = @"D:\CodeProjects\SpiceBot\SpiceBot\NumberStation\NumberAudio";
        
        public NumberGenerator()
        {
            _random = new Random();
        }

        public FileStream GetNumber()
        {
            var number = _random.Next(0, 11);
            var filename = number == 0 ? "bell" : number.ToString();
            return File.OpenRead(Path.Join(CodebaseFileLocation, filename + ".raw"));
        }
    }
}