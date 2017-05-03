using System.Collections.Generic;

namespace TestDesktopDriver.Nuimo.LedMatrix
{
    public class String
    {
        public readonly IList<Frame> frames = new List<Frame>();

        public String(string text, IFont font)
        {
            foreach (var c in text)
            {
                frames.Add(font.GetFrameForCharacter(c));
            }
        }
    }
}
