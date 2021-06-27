using System;
using System.Collections.Generic;
using System.IO;

namespace SpiceBot.NumberStation
{
    internal class NumberGenerator
    {
        private readonly Random _random;
        private readonly Dictionary<string, byte[]> _soundsByName = new ();

        /// <summary>
        /// The location of the audio files relative to the application.
        /// </summary>
        private const string AudioLocation = @"NumberStation\NumberAudio";
        
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
            _soundsByName.Add(s, File.ReadAllBytes(Path.Join(AudioLocation, s + ".raw")));
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