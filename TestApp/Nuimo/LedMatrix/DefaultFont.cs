using System.Collections.Generic;
using Windows.Media.Streaming.Adaptive;

namespace TestApp.Nuimo.LedMatrix
{
    public class DefaultFont : IFont
    {
        private static IFont instance;

        public static IFont Instance => instance ?? (instance = new DefaultFont());

        private IDictionary<char, Frame> Map = new Dictionary<char, Frame>
        {
            // http://www.ffonts.net/DotMatrix-Regular.font
            ['a'] = new Frame(
                "         " +
                "         " +
                "    x    " +
                "   x x   " +
                "  x   x  " +
                " x     x " +
                "x x x x x" +
                "x       x" +
                "x       x"
            ),
            ['b'] = new Frame(
                "         " +
                "         " +
                "x x x x  " +
                " x      x" +
                " x      x" +
                " x  x x  " +
                " x      x" +
                " x      x" +
                "x x x x  "
            ),
            ['c'] = new Frame(
                "         " +
                "         " +
                "  x x x  " +
                "x       x" +
                "x        " +
                "x        " +
                "x        " +
                "x       x" +
                "  x x x  "
            ),
            ['d'] = new Frame(
                "         " +
                "         " +
                "x x x x  " +
                " x      x" +
                " x      x" +
                " x      x" +
                " x      x" +
                " x      x" +
                "x x x x  "
            ),
            ['e'] = new Frame(
                "         " +
                "         " +
                "x x x x x" +
                "x        " +
                "x        " +
                "x x x x  " +
                "x        " +
                "x        " +
                "x x x x x"
            ),
            ['f'] = new Frame(
                "         " +
                "         " +
                "x x x x x" +
                "x        " +
                "x        " +
                "x x x x  " +
                "x        " +
                "x        " +
                "x        "
            ),
            ['g'] = new Frame(
                "         " +
                "         " +
                "  x x x  " +
                "x       x" +
                "x        " +
                "x     x x" +
                "x       x" +
                "x       x" +
                "  x x x  "
            ),
            ['h'] = new Frame(
                "         " +
                "         " +
                "x       x" +
                "x       x" +
                "x       x" +
                "x x x x x" +
                "x       x" +
                "x       x" +
                "x       x"
            ),
        };

        private readonly Frame Unknown = new Frame(
            "         " +
            "         " +
            "         " +
            "         " +
            "         " +
            "         " +
            "         " +
            "         " +
            "         "
            );

        public Frame GetFrameForCharacter(char c)
        {
            var result = Unknown;
            if (Map.ContainsKey(c))
            {
                result = Map[c];
            }

            return result;
        }
    }
}
