using System;
using System.Collections.Generic;
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
            await _pcmStream.WriteAsync(bytes);
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
        private readonly Dictionary<string, byte[]> _soundsByName = new ();

        /// <summary>
        /// The location of the audio files on my personal computer.
        /// Will replace with deployment friendly assembly resource/folder once I'm done debugging.
        /// </summary>
        private const string CodebaseFileLocation = @"D:\CodeProjects\SpiceBot\SpiceBot\NumberStation\NumberAudio";
        
        public NumberGenerator()
        {
            _random = new Random();

            for (var i = 1; i <= 10; i++)
            {
                var num = i.ToString();
                AddSound(num);
            }
            AddSound("bell");
        }

        private void AddSound(string s)
        {
            _soundsByName.Add(s, File.ReadAllBytes(Path.Join(CodebaseFileLocation, s + ".raw")));
        }

        public byte[] GetNumber()
        {
            var number = _random.Next(0, 100);
            var randomBell = _random.Next(6);
            if (number == 0 || randomBell == 5) return _soundsByName["bell"];
            
            var numberAsString = number.ToString("00");
            var outSound = new List<byte>();

            var tensFile = numberAsString[0].ToString();
            if (!tensFile.Equals("0"))
            {
                if (!tensFile.Equals("1")) outSound.AddRange(_soundsByName[tensFile]);
                outSound.AddRange(_soundsByName[10.ToString()]);
            }
            
            var unitsFile = numberAsString[1].ToString();
            if (!unitsFile.Equals("0"))
            {
                outSound.AddRange(_soundsByName[unitsFile]);
            }
            
            return outSound.ToArray();
        }
    }
}