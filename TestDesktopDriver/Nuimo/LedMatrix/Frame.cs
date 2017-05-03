using System;
using System.Collections.Generic;
using System.Linq;

namespace TestDesktopDriver.Nuimo.LedMatrix
{
    public class Frame
    {
        private const int LedCount = 81;

        private static readonly char[] ledOffCharacters = " 0".ToCharArray();

        public Frame(string pattern)
        {
            Bytes = ToBytes(ToBoolArray(pattern));
        }

        public Frame(IReadOnlyCollection<bool> leds)
        {
            Bytes = ToBytes(leds);
        }

        public byte[] Bytes { get; private set; }
    
        private static IEnumerable<IEnumerable<bool>> Chunk(IReadOnlyCollection<bool> matrix, int chunkSize)
        {
            for (var i = 0; i < matrix.Count / chunkSize + 1; i++)
            {
                yield return matrix.Skip(i * chunkSize).Take(chunkSize);
            }
        }

        private static byte[] ToBytes(IReadOnlyCollection<bool> leds)
        {
            return Chunk(leds, 8)
                .Select(chunk => chunk
                    .Select((led, i) => Convert.ToByte(led ? 1 << i : 0))
                    .Aggregate((a, b) => Convert.ToByte(a + b)))
                .ToArray();
        }

        private static bool[] ToBoolArray(string pattern)
        {
            return pattern
                .Substring(0, Math.Min(LedCount, pattern.Length))
                .PadRight(LedCount)
                .ToCharArray()
                .Select(c => (!ledOffCharacters.Contains(c)))
                .ToArray();
        }
    }
}