using UnityEngine;

class AnimatorParameterToggle : StateMachineBehaviour
{
    [SerializeField] string parameterName;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Toggle(animator);
    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Toggle(animator);
    }

    void Toggle(Animator animator)
    {
        var value = animator.GetBool(parameterName);
        animator.SetBool(parameterName, !value);
    }
}