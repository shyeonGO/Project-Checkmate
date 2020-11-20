using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class AnimatorExtension
{
    public static (AnimatorStateInfo current, AnimatorStateInfo next) GetCurrentAndNextAnimatorStateInfo(this Animator animator, int layerIndex)
    {
        return (
            animator.GetCurrentAnimatorStateInfo(layerIndex),
            animator.GetNextAnimatorStateInfo(layerIndex)
            );
    }
}