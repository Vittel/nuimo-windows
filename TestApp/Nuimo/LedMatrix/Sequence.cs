using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp.Nuimo.LedMatrix
{
    public class Sequence
    {

        public Transition Transition { get; set; } = Transition.LeftToRight;

        public IList<String> Strings { get; } = new List<String>();
    }
}
