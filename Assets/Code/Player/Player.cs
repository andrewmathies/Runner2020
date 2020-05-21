using System;
using System.Collections.Generic;

namespace Player {
    public class Player {
        public PlayerState State { get; private set; }
        private Dictionary<StateTransition, PlayerState> transitions;

        public Player() {
            State = PlayerState.Run;
            transitions = new Dictionary<StateTransition, PlayerState>();

            transitions.Add(new StateTransition(PlayerState.Run, Command.StartAttack), PlayerState.Attack);
            transitions.Add(new StateTransition(PlayerState.Attack, Command.EndAttack), PlayerState.Rebound);
            transitions.Add(new StateTransition(PlayerState.Rebound, Command.EndRebound), PlayerState.Run);
        }

        public PlayerState ChangeState(Command command) {
            StateTransition transition = new StateTransition(State, command);
            PlayerState nextState;

            if (!transitions.TryGetValue(transition, out nextState)) {
                throw new System.Exception("Invalid transition: " + State + " -> " + command);
            }

            return nextState;
        }
    }
}