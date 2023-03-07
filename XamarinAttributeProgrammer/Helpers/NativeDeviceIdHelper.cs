using System;

namespace XamarinAttributeProgrammer.Helpers
{
    public static class NativeDeviceIdHelper
    {
        public static Func<object, string> GetIdFromNativeDevice { get; set; }
    }
}