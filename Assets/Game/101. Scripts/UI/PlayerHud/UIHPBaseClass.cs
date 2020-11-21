using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class UIHPBaseClass : MonoBehaviour
{
    public Image mainHpBar;
    public Image subHpBar;
    public float decreaseSpeed;

    protected double currentHp;
    protected double maxHp;

    // Start is called before the first frame update
    protected abstract void Start();

    public void UpdateHpBar()
    {
        SetCurrentHpBarSetting();
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

    protected abstract void SetCurrentHpBarSetting();
}
