using System;

namespace TestDesktopDriver.Nuimo
{
    public interface ICharacteristicNotifier<out TOut> : ICharacteristicReader<TOut>
    {
        event Action<TOut> ValueChanged;
    }
}