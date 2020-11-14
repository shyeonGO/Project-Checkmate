using UnityEngine;
using System.Collections;
using TMPro;

public class PlayerHud : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI hpTextComponent;
    [SerializeField] TextMeshProUGUI staminaTextComponent;
    [SerializeField] TextMeshProUGUI switchPointTextComponent;

    public string HpText
    {
        get => hpTextComponent.ToReferenceNull()?.text;
        private set => hpTextComponent.text = value;
    }

    public string StaminaText
    {
        get => staminaTextComponent.ToReferenceNull()?.text;
        private set => staminaTextComponent.text = value;
    }

    public string SwitchPointText
    {
        get => switchPointTextComponent.ToReferenceNull()?.text;
        private set => switchPointTextComponent.text = value;
    }

    public double HP
    {
        get => float.TryParse(HpText, out var result) ? result : default;
        set => HpText = $"{value:g0}";
    }
    public double Stamina
    {
        get => float.TryParse(StaminaText, out var result) ? result : default;
        set => StaminaText = $"{value:g0}";
    }
    public double SwitchPoint
    {
        get => float.TryParse(SwitchPointText, out var result) ? result : default;
        set => SwitchPointText = $"{value:g0}";
    }
}