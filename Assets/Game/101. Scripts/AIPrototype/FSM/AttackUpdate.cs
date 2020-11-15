using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackUpdate : StateMachineBehaviour
{
    private AIMaster aiMaster;
    public bool continueAngleToPlayer = false;
    public double setAttackDamage;

    // 애니메이션 구조로 볼 때 Enter, Update, Exit는 서로 나눠놓을 필요가 있어보임
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        aiMaster = animator.GetComponent<AIMaster>();
        aiMaster.SetAttackDamage(setAttackDamage);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (continueAngleToPlayer)
        {
            aiMaster.SetEvadeDirection(true);
        }
    }
}
