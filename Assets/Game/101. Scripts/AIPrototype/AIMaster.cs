using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.AI;

public class AIMaster : MonoBehaviour
{
    [Header("Basic Setting")]
    public string bossName;
    public float healthPoint;
    public float setRotationSpeed;
    public bool isFirstStrike = false;
    public Transform rayCastTransform;
    public float guidanceDistance = 1f;
    [SerializeField]
    private BossDamageTriggerManager bossDamageTriggerManager;
    public int changePhase2HealthPoint;

    [Header("Groggy")]
    public float groggy;
    private float saveGroggy;
    public float groggyContinuingTime;
    public float groggyRecoverySpeed;
    public float groggyRecoveryInterval;

    [Header("Infect")]
    public float infect;
    private float saveInfect;
    public float infectContinuingTime;
    public float infectRecoverySpeed;
    public float infectRecoveryInterval;

    [Header("Distance Setting")]
    public float closeRangeAttackDistance;
    public float longRangeAttackDistance;
    public float trackingDistance;

    [Header("Debug")]
    [SerializeField]
    private bool DebugOn = true;
    [SerializeField]
    private Vector3 AgentNextPostiion;

    private NavMeshAgent agent;
    private Animator anim;
    private GameObject player;
    private float speedSave;

    public Animator GetBossAnimator
    {
        get
        {
            return anim;
        }
    }

    public bool isEvade = false;
    public bool isMove = true;

    private void Awake()
    {
        if (bossDamageTriggerManager == null)
        {
            bossDamageTriggerManager = GetComponent<BossDamageTriggerManager>();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");

        agent.updatePosition = false;
        agent.updateRotation = false;

        // 추가됨
        saveGroggy = 0;
        saveInfect = 0;

        speedSave = agent.speed;
    }

    // Update is called once per frame
    void Update()
    {
        AttackDistance();
        //CustomLookAt(player.transform.position);
        AgentNextPostiion = agent.nextPosition;
    }

    private void FixedUpdate()
    {
        if (isFirstStrike)
        {
            if (anim.GetInteger("attackCode") == 0 && isMove)
            {
                SwitchingRootMotion();
            }
            if (isEvade == false)
            {
                NavMeshAgentGuidance();
            }
        }
        //Debug.Log(agent.nextPosition);
    }

    private void AttackDistance()
    {
        float playerDistance = Vector3.Distance(transform.position, player.transform.position);

        anim.SetFloat("playerDistance", playerDistance);

        if (playerDistance <= closeRangeAttackDistance)
        {
            //DebugString("Close Range Attack");
            anim.SetBool("closeRangeAttack", true);
            anim.SetBool("longRangeAttack", false);
            anim.SetBool("trackingDistance", false);
        }
        else if (playerDistance > closeRangeAttackDistance && playerDistance <= longRangeAttackDistance)
        {
            //DebugString("Long Range Attack");
            anim.SetBool("closeRangeAttack", false);
            anim.SetBool("longRangeAttack", true);
            anim.SetBool("trackingDistance", false);
        }
        else if (playerDistance > longRangeAttackDistance && playerDistance <= trackingDistance)
        {
            //DebugString("Tracking");
            anim.SetBool("closeRangeAttack", false);
            anim.SetBool("longRangeAttack", false);
            anim.SetBool("trackingDistance", true);
        }
        else
        {
            anim.SetBool("closeRangeAttack", false);
            anim.SetBool("longRangeAttack", false);
            anim.SetBool("trackingDistance", false);
        }
    }

    public void SwitchingRootMotion()
    {
        Vector3 newTransformPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        Vector3 newAgentPosition = new Vector3(agent.nextPosition.x, transform.position.y, agent.nextPosition.z);

        if (Vector3.Distance(newTransformPosition, newAgentPosition) >= 0.3f)
        {
            CustomLookAt(newAgentPosition, setRotationSpeed);
        }
        else
        {

        }
    }

    /// <summary>
    /// 함수의 기능이 SetEvadeDirection()과 비슷함
    /// 추후에 SetEvadeDirection과 함께 반드시 수정이 필요함
    /// </summary>
    private void NavMeshAgentGuidance()
    {
        //float speed;
        Vector3 evadeDirection = (player.transform.position - transform.position).normalized;
        agent.nextPosition = transform.position + (evadeDirection * guidanceDistance);

        //if (Vector3.Distance(transform.position, agent.nextPosition) >= guidanceDistance)
        //{
        //    speed = 0f;
        //}
        //else
        //{
        //    speed = speedSave;
        //}
        agent.speed = Mathf.Lerp(agent.speed, 0, Time.deltaTime * 3f);
    }

    public void AttackSequence()
    {
        isMove = false;
    }

    public void SetAngleToPlayer(float rotationSpeed = 20)
    {
        CustomLookAt(player.transform.position, rotationSpeed);
    }

    public void TrackingPlayer()
    {
        // 플레이어 사이의 장애물이 있으면 버그가 발생함
        //Vector3 evadeDirection = (player.transform.position - transform.position).normalized;
        //agent.nextPosition = transform.position + evadeDirection;

        SwitchingRootMotion();
        agent.destination = player.transform.position;
    }

    public void SetAttackDamage(double value)
    {
        bossDamageTriggerManager.setAttackDamage = value;
    }

    /// <summary>
    /// 정식 사용중
    /// </summary>
    /// <param name="phase"></param>
    public void ChangePhase(int phase)
    {
        if (healthPoint <= changePhase2HealthPoint)
        {
            anim.SetInteger("Phase", phase);
        }
    }

    public void SetBool(string name)
    {
        anim.SetBool(name, true);
    }

    #region Evade Function
    public void SetEvadePosition(out bool value)
    {
        SetEvadeDirection();

        isMove = true;

        value = isEvade;

        Vector3 evadeDirection = (transform.position - player.transform.position).normalized;

        RaycastHit hit;
        Ray ray = new Ray(rayCastTransform.position, rayCastTransform.forward);

        Debug.DrawRay(ray.origin, ray.direction * 3f, Color.red);

        //agent.destination = evadeDirection * 10f;
        if ((Physics.Raycast(ray, out hit, 3f) && !hit.collider.CompareTag("Player")) || Vector3.Distance(transform.position, player.transform.position) >= 20)
        {
            isEvade = false;
            value = isEvade;
        }
        else
        {
            isEvade = true;
            value = isEvade;
        }
    }

    public void SetEvadeDirection(bool isReverse = false)
    {
        Vector3 evadeDirection;
        if (isReverse)
        {
            evadeDirection = (player.transform.position - transform.position).normalized;
        }
        else
        {
            evadeDirection = (transform.position - player.transform.position).normalized;
        }
        agent.nextPosition = transform.position + (evadeDirection * guidanceDistance);
        agent.destination = agent.nextPosition + (transform.forward * 5f);
    }
    #endregion

    #region Utilities Function
    private float GetTargetAngle(Vector3 target)
    {
        Vector3 targetDirection = target - transform.position;
        float angle = Mathf.Acos(Vector3.Dot(targetDirection, transform.forward));

        return angle;
    }


#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (DebugOn)
        {
            Handles.color = new Color(1, 0, 0, 0.3f);
            Handles.DrawSolidDisc(transform.position, transform.up, closeRangeAttackDistance);

            Handles.color = new Color(0, 0, 1, 0.2f);
            Handles.DrawSolidDisc(transform.position, transform.up, longRangeAttackDistance);

            Handles.color = new Color(0, 0, 0, 0.2f);
            Handles.DrawSolidDisc(transform.position, transform.up, trackingDistance);
        }
    }
#endif

    private void DebugString(string str)
    {
        if (DebugOn)
        {
            Debug.Log(str);
        }
    }

    private void CustomLookAt(Vector3 target, float rotationSpeed = 20, bool isReverse = false)
    {
        Vector3 direction;
        if (isReverse)
        {
            direction = transform.position - target;
        }
        else
        {
            direction = target - transform.position;
        }
        direction.y = transform.position.y;
        Quaternion toRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, Time.deltaTime * rotationSpeed);
    }
    #endregion
}
