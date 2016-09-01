using System;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Bluetooth.GenericAttributeProfile;

namespace NuimoSDK
{
    internal static class GattValueChangedEventArgsExtensions
    {
        public static NuimoGestureEvent ToButtonEvent(this GattValueChangedEventArgs changedValue)
        {
            var value = changedValue.CharacteristicValue.ToArray()[0];

            return new NuimoGestureEvent(
                value == 1
                    ? NuimoGesture.ButtonPress
                    : NuimoGesture.ButtonRelease,
                value);
        }

        public static NuimoGestureEvent ToSwipeEvent(this GattValueChangedEventArgs changedValue)
        {
            var value = changedValue.CharacteristicValue.ToArray()[0];
            switch (value)
            {
                case 0: return new NuimoGestureEvent(NuimoGesture.SwipeLeft, value);
                case 1: return new NuimoGestureEvent(NuimoGesture.SwipeRight, value);
                case 2: return new NuimoGestureEvent(NuimoGesture.SwipeUp, value);
                case 3: return new NuimoGestureEvent(NuimoGesture.SwipeDown, value);
                default: return null;
            }
        }

        public static NuimoGestureEvent ToRotationEvent(this GattValueChangedEventArgs changedValue)
        {
            return new NuimoGestureEvent(
                NuimoGesture.Rotate,
                BitConverter.ToInt16(changedValue.CharacteristicValue.ToArray(),
                0));
        }

        public static NuimoGestureEvent ToFlyEvent(this GattValueChangedEventArgs changedValue)
        {
            var value = changedValue.CharacteristicValue.ToArray()[0];
            switch (value)
            {
                case 0: return new NuimoGestureEvent(NuimoGesture.FlyLeft, value);
                case 1: return new NuimoGestureEvent(NuimoGesture.FlyRight, value);
                case 2: return new NuimoGestureEvent(NuimoGesture.FlyTowards, value);
                case 3: return new NuimoGestureEvent(NuimoGesture.FlyBackwards, value);
                case 4: return new NuimoGestureEvent(NuimoGesture.FlyUpDown, changedValue.CharacteristicValue.ToArray()[1]);
                default: return null;
            }
        }
    }
}