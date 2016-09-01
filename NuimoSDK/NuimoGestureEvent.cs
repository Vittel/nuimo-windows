namespace NuimoSDK
{
    public class NuimoGestureEvent
    {
        public NuimoGesture Gesture { get; }

        public int Value { get; }

        public NuimoGestureEvent(NuimoGesture gesture, int value)
        {
            Gesture = gesture;
            Value = value;
        }
    }
}
