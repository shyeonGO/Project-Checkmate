using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorParameterIntSetter : StateMachineBehaviour
{
    [SerializeField] string parameterName;
    [SerializeField] int value;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetInteger(parameterName, value);
    }
}
