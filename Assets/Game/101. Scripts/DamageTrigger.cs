using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 무기나 타점에 장착하여 사용
/// </summary>
[RequireComponent(typeof(Collider))]
public abstract class DamageTrigger : MonoBehaviour
{
    [SerializeField] string[] targetTags = new string[1];
    [SerializeField] LayerMask targetLayers;
    [Tooltip("트리거만 체크할지 여부입니다.")]
    [SerializeField] bool checkOnlyTrigger;

    Collider thisCollider;

    public string[] TargetTags { get => this.targetTags; set => this.targetTags = value; }
    public LayerMask TargetLayers { get => this.targetLayers; set => this.targetLayers = value; }
    public bool CheckOnlyTrigger { get => this.checkOnlyTrigger; set => this.checkOnlyTrigger = value; }

    // 중복 검사용 리스트
    List<Collider> checkedColliders = new List<Collider>();

    private void Awake()
    {
        thisCollider = GetComponent<Collider>();
        thisCollider.isTrigger = true;

        EndTrigger();
    }

    /// <summary>
    /// 트리거 체크를 시작합니다.
    /// </summary>
    public void StartTrigger()
    {
        thisCollider.enabled = true;
    }

    /// <summary>
    /// 트리거 체크를 끝냅니다.
    /// </summary>
    public void EndTrigger()
    {
        thisCollider.enabled = false;
        checkedColliders.Clear();
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (checkOnlyTrigger ? other.isTrigger : true)
        {
            if (!checkedColliders.Contains(other))
            {
                checkedColliders.Add(other);
                if (other.CompareTags(targetTags) && other.gameObject.MatchLayer(targetLayers))
                {
                    if (other.TryGetComponent<DamageHandler>(out var damageHandler))
                    {
                        DealingDamage(damageHandler);
                    }
                }
            }
        }
    }

    /// <summary>
    /// 데미지 전달
    /// </summary>
    protected virtual void DealingDamage(DamageHandler damageHandler)
    {
        var damageData = GetDamageData();
        if (damageData.Trigger == null)
        {
            damageData.Trigger = this;
        }

        damageHandler.DamageHandle(damageData);
    }

    /// <summary>
    /// 전달될 데미지를 연산합니다.
    /// </summary>
    /// <returns></returns>
    public abstract DamageData GetDamageData();
}
