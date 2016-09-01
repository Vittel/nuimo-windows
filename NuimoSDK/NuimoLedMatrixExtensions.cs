using System;
using System.Collections.Generic;
using System.Linq;

namespace NuimoSDK
{
    internal static class NuimoLedMatrixExtensions
    {
        public static byte[] GattBytes(this NuimoLedMatrix matrix)
        {
            return matrix.Leds
                .Chunk(8)
                .Select(chunk => chunk
                    .Select((led, i) => Convert.ToByte(led ? 1 << i : 0))
                    .Aggregate((a, b) => Convert.ToByte(a + b)))
                .ToArray();
        }

        public static IEnumerable<IEnumerable<bool>> Chunk(this bool[] matrix, int chunkSize)
        {
            for (var i = 0; i < matrix.Length / chunkSize + 1; i++)
            {
                yield return matrix.Skip(i * chunkSize).Take(chunkSize);
            }
        }
    }
}