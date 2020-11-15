// 임시용
// 나중에 삭제될 예정입니다
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TempBossUI : MonoBehaviour
{
    public Text bossHp;
    public AIMaster aiMaster;

    public void UpdateValue()
    {
        bossHp.text = "Boss : " + aiMaster.healthPoint.ToString();
    }
}
