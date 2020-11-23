using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class OverlapComponenet : MonoBehaviour
{
    public Collider[] coll;
    public List<GameObject> gb;
    public Button btn;
    public LayerMask targetLayer;

    // Start is called before the first frame update
    void Start()
    {
        btn.onClick.AddListener(SetOverlap);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 현재 레이어가 셋팅되어 있지 않아서 계산속도는 정말정말정말 처참함
    /// 이유는 일단 주변에 있는 모든 충돌체를 가져오기 때문임
    /// 나중에 레이어를 추가함으로서 어느정도 보완이 가능할 것으로 보임
    /// </summary>
    public void SetOverlap()
    {
        gb.Clear();
        coll = Physics.OverlapCapsule(transform.position, transform.position, 10f, targetLayer);

        for (int i = 0; i < coll.Length; i++)
        {
            if (coll[i].CompareTag("Enemy"))
            {
                gb.Add(coll[i].gameObject);
            }
        }
    }

    public void OnDrawGizmosSelected()
    {
        Handles.color = new Color(1, 0, 0, 0.3f);
        Handles.DrawSolidDisc(transform.position, transform.up, 10);
    }
}
