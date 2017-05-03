namespace TestApp.Nuimo
{
    public interface ICharacteristicReader<out TOut>
    {
        TOut CurrentValue { get; }

        byte[] CurrentRawValue { get; }
    }
}