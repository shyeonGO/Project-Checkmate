using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEnter : StateMachineBehaviour
{
    private AIMaster aiMaster;
    public bool isSetAngleToPlayer = false;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        aiMaster = animator.GetComponent<AIMaster>();
        aiMaster.AttackSequence();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (isSetAngleToPlayer)
        {
            aiMaster.SetAngleToPlayer();
        }
    }
}
