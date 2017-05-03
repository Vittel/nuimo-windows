using System;

namespace TestDesktopDriver.Nuimo
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
