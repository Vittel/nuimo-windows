using System.Collections.Generic;

namespace TestDesktopDriver.Nuimo.LedMatrix
{
    public class Sequence
    {

        public Transition Transition { get; set; } = Transition.LeftToRight;

        public IList<String> Strings { get; } = new List<String>();
    }
}
