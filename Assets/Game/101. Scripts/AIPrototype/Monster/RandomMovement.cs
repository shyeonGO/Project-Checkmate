using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(AIMaster))]
public class RandomMovement : MonoBehaviour
{
    [SerializeField]
    private AIMaster aiMaster;
    [SerializeField]
    private NavMeshAgent agent;

    public Vector3 startPos;
    public float size;

    // Start is called before the first frame update
    void Awake()
    {
        if (aiMaster == null)
        {
            aiMaster = GetComponent<AIMaster>();
        }
        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
        }

        InitRandomPos();
    }

    public void InitRandomPos()
    {
        agent.destination = new Vector3(Random.Range(-size, size), transform.position.y, Random.Range(-size, size));
    }

    public void RandomMove()
    {
        aiMaster.SwitchingRootMotion();
        if (Vector3.Distance(agent.destination, agent.transform.position) <= agent.stoppingDistance)
        {
            agent.destination = new Vector3(Random.Range(-size, size), transform.position.y, Random.Range(-size, size));
        }
        else
        {
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Handles.color = new Color(1, 1, 1, 0.1f);
        Handles.DrawSolidDisc(startPos, transform.up, size);
    }
#endif
}
