using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerHP : MonoBehaviour
{
    public Image mainHpBar;
    public Image subHpBar;
    public float decreaseSpeed;

    public double currentHp;
    public double maxHp;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void DamagePlayer()
    {
        currentHp -= Random.Range(1, 30);
        UpdateHpBar();
    }

    public void UpdateHpBar()
    {
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
