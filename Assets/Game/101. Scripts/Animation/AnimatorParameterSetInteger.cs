using UnityEngine;

public class AnimatorParameterSetInteger : StateMachineBehaviour
{
    [SerializeField] string parameterName;
    [SerializeField] int value;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetInteger(parameterName, value);
    }
}
