namespace CollectdClient.Core
{
    internal static class Protocol
    {
        internal const int TypeHost = 0x0000;
        internal const int TypeTime = 0x0001;
        internal const int TypeTimeHires = 0x0008;
        internal const int TypePlugin = 0x0002;
        internal const int TypePluginInstance = 0x0003;
        internal const int TypeType = 0x0004;
        internal const int TypeTypeInstance = 0x0005;
        internal const int TypeValues = 0x0006;
        internal const int TypeInterval = 0x0007;
        internal const int TypeIntervalHires = 0x0009;
        internal const int TypeMessage = 0x0100;
        internal const int TypeSeverity = 0x0101;

        internal const int UInt8Len = 1;
        internal const int UInt16Len = UInt8Len * 2;
        internal const int UInt32Len = UInt16Len * 2;
        internal const int UInt64Len = UInt32Len * 2;
        internal const int HeaderLen = UInt16Len * 2;
                       
        internal const int BufferSize = 1024;
    }
}
