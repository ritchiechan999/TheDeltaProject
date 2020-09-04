using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class tdEntityStateChanger : StateMachineBehaviour {
    public tdEntityState OnEnterState = tdEntityState.Unassigned;
    public tdEntityState OnExitState = tdEntityState.Unassigned;
    //to test if possible -geoff
    tdComboHandler _onCombo;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (OnEnterState != tdEntityState.Unassigned) {
            string csString = $"td{OnEnterState}State";
            Type cs = Type.GetType(csString);
            tdIBrainFSM anim = animator.GetComponent<tdIBrainFSM>();
            anim.ChangeState(cs, new object[] { });
        }

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (OnExitState != tdEntityState.Unassigned) {
            _onCombo = animator.GetComponent<tdComboHandler>();
            if (!_onCombo.IsOnCombo) {
                string csString = $"td{OnExitState}State";
                Type cs = Type.GetType(csString);
                tdIBrainFSM anim = animator.GetComponent<tdIBrainFSM>();
                anim.ChangeState(cs, new object[] { });
            }
        }
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
