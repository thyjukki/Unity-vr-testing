using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour {


	public Soldier soldier;
	public Animator anim;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

    }

    public void EnterState(AIState state) {
        switch (state) {
            case AIState.Patrol:
                soldier.StartPatrol();
                break;
            case AIState.Chase:
                break;
            case AIState.Alert:
                break;
            default:
                break;
        }

    }

    public void ExitState(AIState state) {
        switch (state) {
            case AIState.Patrol:
                soldier.EndPatrol();
                break;
            case AIState.Chase:
                break;
            case AIState.Alert:
                break;
            default:
                break;
        }

    }

    public void UpdateState(AIState state) {
        switch (state) {
            case AIState.Patrol:
                soldier.UpdatePatrol();
                break;
            case AIState.Chase:
                break;
            case AIState.Alert:
                break;
            default:
                break;
        }
    }
}

public enum AIState {
    Patrol,
    Chase,
    Alert
}