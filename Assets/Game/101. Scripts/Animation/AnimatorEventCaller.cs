using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class AnimatorEventCaller : StateMachineBehaviour
{
    [SerializeField] string eventName;

    [SerializeField] bool callByEnter = true;
    [SerializeField] bool callByExit = false;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (callByEnter)
            animator.SendMessage(eventName);
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (callByExit)
            animator.SendMessage(eventName);
    }
}