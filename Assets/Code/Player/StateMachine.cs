using UnityEngine;

namespace Player {
    public abstract class StateMachine : MonoBehaviour {
        public State State;

        public void SetState(State newState) {
            State = newState;
            StartCoroutine(State.Start());
        }
    }
}