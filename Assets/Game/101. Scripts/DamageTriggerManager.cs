

using Boo.Lang;
using UnityEngine;
/// <summary>
/// 애니메이션에서 발생된 이벤트를 처리합니다.
/// </summary>
class DamageTriggerManager : MonoBehaviour
{
    [SerializeField] DamageTrigger[] _damageTriggers;

    List<DamageTrigger> damageTriggers;

    public List<DamageTrigger> DamageTriggers { get => this.damageTriggers; set => this.damageTriggers = value; }

    void Awake()
    {
        damageTriggers = new List<DamageTrigger>(_damageTriggers);
        _damageTriggers = null;
    }

    public void StartTrigger()
    {
        foreach (var damageTrigger in damageTriggers)
        {
            damageTrigger.StartTrigger();
        }
    }

    public void EndTrigger()
    {
        foreach (var damageTrigger in damageTriggers)
        {
            damageTrigger.EndTrigger();
        }
    }

    void DamageTrigger_StartTrigger()
    {
        StartTrigger();
    }

    void DamageTrigger_EndTrigger()
    {
        EndTrigger();
    }
}