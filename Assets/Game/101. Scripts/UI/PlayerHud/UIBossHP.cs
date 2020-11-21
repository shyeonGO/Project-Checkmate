using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBossHP : MonoBehaviour
{
    public AIMaster aiMaster;

    public Image mainHpBar;
    public Image subHpBar;
    public float decreaseSpeed;

    public double currentHp;
    public double maxHp;

    private void Start()
    {
        maxHp = aiMaster.healthPoint;
    }

    public void UpdateHpBar()
    {
        currentHp = aiMaster.healthPoint;
        mainHpBar.fillAmount = (float)(currentHp / maxHp);
        StartCoroutine(LerpSubHpBar());
    }

    IEnumerator LerpSubHpBar()
    {
        while (Mathf.Abs(mainHpBar.fillAmount - subHpBar.fillAmount) > 0.001f)
        {
            subHpBar.fillAmount = Mathf.Lerp(subHpBar.fillAmount, mainHpBar.fillAmount, Time.deltaTime * decreaseSpeed);
            yield return null;
        }
    }
}
