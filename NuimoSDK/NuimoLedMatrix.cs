using System;
using System.Linq;

namespace NuimoSDK
{
    public class NuimoLedMatrix
    {
        public bool[] Leds { get; }

        private const int LedCount = 81;

        private readonly char[] ledOffCharacters = " 0".ToCharArray();

        public NuimoLedMatrix(string pattern)
        {
            Leds = pattern
                .Substring(0, Math.Min(LedCount, pattern.Length))
                .PadRight(LedCount)
                .ToCharArray()
                .Select(c => (!ledOffCharacters.Contains(c)))
                .ToArray();
        }
    }
}