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
    public float rotationSpeed;
    public bool isFirstStrike = false;
    public Transform rayCastTransform;

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
    public bool isMove = true;

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
        if (isFirstStrike && anim.GetInteger("attackCode") == 0 && isMove)
        {
            SwitchingRootMotion();
        }
        NavMeshAgentGuidance();
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
            CustomLookAt(newAgentPosition);
        }
        else
        {

        }
    }

    private void NavMeshAgentGuidance()
    {
        float speed;
        if (Vector3.Distance(transform.position, agent.nextPosition) >= 1f)
        {
            speed = 0f;
        }
        else
        {
            speed = 5f;
        }
        agent.speed = Mathf.Lerp(agent.speed, speed, Time.deltaTime * 3f);
    }

    public bool SetEvadePosition()
    {
        isMove = true;
        Vector3 evadeDirection = (transform.position - player.transform.position).normalized;

        RaycastHit hit;
        Ray ray = new Ray(rayCastTransform.position, rayCastTransform.forward);

        Debug.DrawRay(ray.origin, ray.direction * 3f, Color.red);

        //agent.destination = evadeDirection * 10f;

        if ((Physics.Raycast(ray, out hit, 3f) && !hit.collider.CompareTag("Player")) || Vector3.Distance(transform.position, player.transform.position) >= 10)
        {
            DebugString("Evade End");
            return false;
        }
        else
        {
            DebugString("Evading");
            return true;
        }
    }

    public void AttackSequence()
    {
        isMove = false;
    }

    public void SetAngleToPlayer()
    {
        CustomLookAt(player.transform.position);
    }

    public void SetEvadeDirection()
    {
        Vector3 evadeDirection = (transform.position - player.transform.position).normalized;
        agent.nextPosition = transform.position + evadeDirection;
        agent.destination = evadeDirection * 10f;
    }

    public void TrackingPlayer()
    {
        agent.destination = player.transform.position;
    }

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

    private void CustomLookAt(Vector3 target, bool isReverse = false)
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
