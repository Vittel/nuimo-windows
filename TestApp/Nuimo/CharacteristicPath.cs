using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp.Nuimo
{
    public struct CharacteristicPath
    {
        public Guid service;
        public Guid characteristic;

        public CharacteristicPath(Guid service, Guid characteristic)
        {
            this.service = service;
            this.characteristic = characteristic;
        }
    }
}
