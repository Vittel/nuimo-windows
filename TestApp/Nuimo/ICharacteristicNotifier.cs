using System;

namespace TestApp.Nuimo
{
    public interface ICharacteristicNotifier<out TOut> : ICharacteristicReader<TOut>
    {
        event Action<TOut> ValueChanged;
    }
}