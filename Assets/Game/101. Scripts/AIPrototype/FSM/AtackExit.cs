using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtackExit : StateMachineBehaviour
{
    private AIMaster aiMaster;
    public bool continueAngleToPlayer = false;

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetInteger("closeAttackCode", 0);
        if (continueAngleToPlayer)
        {
            aiMaster.isMove = true;
        }
    }
}
