using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnimatorTriggerManager : MonoBehaviour
{
    Dictionary<int, float> triggerTimes = new Dictionary<int, float>();
    Dictionary<int, float> modifieredTriggerTimes = new Dictionary<int, float>();
    [SerializeField] Animator animator;

    List<int> resetTriggerIdList = new List<int>();
    private void Awake()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        var deltaTime = Time.deltaTime;
        foreach (var triggerTime in triggerTimes)
        {
            var id = triggerTime.Key;
            var time = triggerTime.Value;

            if (time == 0)
            {
                resetTriggerIdList.Add(id);
            }
            else
            {
                Mathx.TimeToZero(ref time, deltaTime);
            }

            modifieredTriggerTimes[id] = time;
        }

        //swap
        SwapTriggerTime();

        foreach (var resetTriggerId in resetTriggerIdList)
        {
            ResetTrigger(resetTriggerId);
        }
        resetTriggerIdList.Clear();
    }

    void SwapTriggerTime()
    {
        var tempTriggerTimes = triggerTimes;
        triggerTimes = modifieredTriggerTimes;
        modifieredTriggerTimes = tempTriggerTimes;

        modifieredTriggerTimes.Clear();
    }

    public void SetTrigger(int id, float time)
    {
        triggerTimes[id] = time;
        animator.SetTrigger(id);
    }

    public void SetTrigger(string name, float time)
    {
        SetTrigger(Animator.StringToHash(name), time);
    }

    public void ResetTrigger(int id)
    {
        triggerTimes.Remove(id);
        animator.ResetTrigger(id);
    }

    public void ResetTrigger(string name)
    {
        ResetTrigger(Animator.StringToHash(name));
    }
}