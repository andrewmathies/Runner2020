namespace Player {
    public class StateTransition {
        readonly PlayerState CurrentState;
        readonly Command Command;

        public StateTransition(PlayerState currentState, Command command) {
            this.CurrentState = currentState;
            this.Command = command;
        }

        // we want any entries in our dictionary with the same state and command to be considered equal
        // we have to implement these methods to get that behavior. idk why we need magic prime numbers 

        public override int GetHashCode() {
            return 17 + 31 * CurrentState.GetHashCode() + 31 * Command.GetHashCode();
        }

        public override bool Equals(object obj) {
            StateTransition other = obj as StateTransition;
            return other != null && this.CurrentState == other.CurrentState && this.Command == other.Command;
        }
    }
}