using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    public State currentState;

    public bool canSeePlayer;
    public bool canHearPlayer;

    // Update is called once per frame
    void Update()
    {
        RunStateMachine();
    }

    private void RunStateMachine()
    {
        State nextState = currentState?.RunCurrentState();
        if(nextState != null)
        {
            SwitchToTheNextState(nextState);
        }
    }

    private void SwitchToTheNextState(State nextState)
    {
        currentState = nextState;
    }

    // Rabbit see and hear player colliders, set in the Player Cat's 'RabbitCollide' script
    // So that it is independent of the state manager
        public bool GetCanSeePlayer()
        {
            return canSeePlayer;
        }

        public void SetCanSeePlayer(bool set)
        {
            canSeePlayer = set;
        }

        public bool GetCanHearPlayer()
        {
            return canHearPlayer;
        }

        public void SetCanHearPlayer(bool set)
        {
            canHearPlayer = set;
        }
}
