using System.Threading;
using UnityEngine;

class PlayerAttackingStateOn : StateMachineBehaviour
{
    static int stack;

    [SerializeField] string parameterName = "isAttacking";

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        stack++;
        SetBool(animator);
    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        stack--;
        SetBool(animator);
    }

    void SetBool(Animator animator)
    {
        animator.SetBool(parameterName, stack > 0);
    }
}