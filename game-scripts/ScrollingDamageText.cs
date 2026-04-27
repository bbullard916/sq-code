
using UnityEngine;
using System.Collections;
using System.Linq;
using BLINK.RPGBuilder.UIElements;
using BLINK.RPGBuilder.Utility;
using TMPro;
using Guirao.UltimateTextDamage;

public class ScrollingDamageText : MonoBehaviour
{
    public UltimateTextDamageManager _UltimateTextDamageManager;
    private void OnEnable()
    {
        CombatEvents.DamageDealt += ShowDamageText;
        CombatEvents.Healed += ShowHealText;
    }

    private void OnDisable()
    {
        CombatEvents.DamageDealt -= ShowDamageText;
        CombatEvents.Healed -= ShowHealText;
    }

    public void Start()
    {
        _UltimateTextDamageManager = GameObject.FindGameObjectWithTag("DamageText").GetComponent<UltimateTextDamageManager>();
        _UltimateTextDamageManager.GetComponent<Canvas>().worldCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }
    private void ShowDamageText(CombatCalculations.DamageResult result)
    {
        Debug.Log("Result:" + result.caster);
        //if (!result.caster.IsPlayer() && !result.target.IsPlayer() && (!result.caster.IsPet() && result != null)) return;
        string message = "";
        switch (result.DamageActionType)
        {
            case "Physical" when result.target == GameState.playerEntity:
            case "Magical" when result.target == GameState.playerEntity:
            case "Physical_CRITICAL" when result.target == GameState.playerEntity:
            case "Magical_CRITICAL" when result.target == GameState.playerEntity:
            case "Neutral" when result.target == GameState.playerEntity:
                message = "" + (int)result.DamageAmount;
                break;
            case "Physical":
            case "Magical":
            case "Physical_CRITICAL":
            case "Magical_CRITICAL":
            case "Neutral":
                message = "" + (int)result.DamageAmount;
                break;
            case "THORN" when result.target.IsPlayer():
            case "THORN":
                message = "" + result.DamageAmount;
                break;
            case "BLOCKED" when result.target.IsPlayer():
            case "BLOCKED":
                message = result.DamageAmount + " (Blocked " +
                          (result.DamageBlockedActively + result.DamageBlockedPassively) + ")";
                break;
            case "DODGED" when result.target.IsPlayer():
            case "DODGED":
                message = "Dodged";
                break;
        }
        //Debug.Log("Result:" + result.DamageActionType);
        //Debug.Log("Result:" + result.DamageAmount);
        if (result.DamageActionType == "Neutral")
        {
            // do nothing since its not related to neutral
        }
        else
        {
            ScreenEventHandler(result.DamageActionType, message, result.target.gameObject);
        }
    }
    private void ShowHealText(CombatCalculations.HealResult result)
    {
        string message = "";
        switch (result.HealActionType)
        {
            case "HEAL" when result.target.IsPlayer():
            case "HEAL_CRITICAL" when result.target.IsPlayer():
                message = "" + (int)result.HealAmount;
                break;
            case "HEAL":
            case "HEAL_CRITICAL":
                message = "" + (int)result.HealAmount;
                break;
        }

    }
    private void ScreenEventHandler(string eventType, string message, GameObject target)
    {
        if (target.tag == "Player")
        {
            if (eventType == "Physical" || eventType == "Magical" || eventType == "Neutral")
            {
                _UltimateTextDamageManager.Add(message, target.transform.Find("DamageText").transform, "default");
            }
            else if (eventType == "Physical_CRITICAL")
            {
                _UltimateTextDamageManager.Add(message, target.transform.Find("DamageText").transform, "critical");
            }
            else if (eventType == "HEAL")
            {
                _UltimateTextDamageManager.Add(message, target.transform.Find("DamageText").transform, "default");
            }
            else if (eventType == "HEAL_CRITICAL")
            {
                _UltimateTextDamageManager.Add(message, target.transform.Find("DamageText").transform, "default");
            }
        }
        else
        {
            if (eventType == "Physical" || eventType == "Magical" || eventType == "Neutral")
            {
                _UltimateTextDamageManager.Add(message, target.transform.Find(target.name + "(Clone)/DamageText").transform, "default");
                //StartCoroutine(FlashEnemyDamageHighlight(target));
            }
            else if (eventType == "Physical_CRITICAL")
            {
                _UltimateTextDamageManager.Add(message, target.transform.Find(target.name + "(Clone)/DamageText").transform, "critical");
                //StartCoroutine(FlashEnemyDamageHighlight(target));
            }
            else if (eventType == "HEAL")
            {
                _UltimateTextDamageManager.Add(message, target.transform.Find(target.name + "(Clone)/DamageText").transform, "default");
            }
            else if (eventType == "HEAL_CRITICAL")
            {
                _UltimateTextDamageManager.Add(message, target.transform.Find(target.name + "(Clone)/DamageText").transform, "default");
            }
        }
    }
}
