using System.IO;

namespace Midi {
    public class SysExEvent : Event {
        public SysExEvent(BinaryReader reader) {
            ulong length = reader.DecodeUInt64();
            reader.ReadBytes((int) length);
        }
    }
}