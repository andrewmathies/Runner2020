
namespace Midi {
    // http://www.music.mcgill.ca/~ich/classes/mumt306/StandardMIDIfileformat.html#BMA1_
    public enum MidiEventType {
        NoteOff,
        NoteOn,
        PolyphonicKeyPressure,
        ControlChange,
        ProgramChange,
        ChannelPressure,
        PitchWheelChange,
        ChannelModeMessage,
        SystemExclusive,
        SongPositionPointer,
        SongSelect,
        TuneRequest,
        EndOfExclusive,
        TimingClock,
        Start,
        Continue,
        Stop,
        ActiveSensing,
        Reset
    }
}