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

    private NavMeshAgent agent;
    private Animator anim;
    private GameObject player;

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
        agent.destination = player.transform.position;
        //transform.Translate(agent.nextPosition.normalized * 5 * Time.deltaTime);
        //CustomLookAt(player.transform.position);
        SwitchingRootMotion();
        DebugString(agent.pathPending.ToString());
    }

    private void AttackDistance()
    {
        float playerDistance = Vector3.Distance(transform.position, player.transform.position);

        if (playerDistance <= closeRangeAttackDistance)
        {
            DebugString("Close Range Attack");
        }
        else if (playerDistance > closeRangeAttackDistance && playerDistance <= longRangeAttackDistance)
        {
            DebugString("Long Range Attack");
        }
        else if (playerDistance > longRangeAttackDistance && playerDistance <= trackingDistance)
        {
            DebugString("Tracking");
        }
    }

    //private void OnAnimatorMove()
    //{
    //    DebugString("AnimationActive");
    //}

    private void SwitchingRootMotion()
    {
        Vector3 newTransformPosition = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 newAgentPosition = new Vector3(agent.nextPosition.x, 0, agent.nextPosition.z);

        if (Vector3.Distance(newTransformPosition, newAgentPosition) >= 0.5f)
        {
            anim.SetBool("isMove", true);
            CustomLookAt(newAgentPosition);
        }
        else
        {
            anim.SetBool("isMove", false);
        }
    }


    #region Utilities Funtion
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

    private void CustomLookAt(Vector3 target)
    {
        Vector3 direction = target - transform.position;
        direction.y = transform.position.y;
        Quaternion toRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, Time.deltaTime * rotationSpeed);
    }
    #endregion
}
