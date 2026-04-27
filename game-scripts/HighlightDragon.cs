using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightDragon : MonoBehaviour
{

    private CanvasGroup _CanvasGroup;
    private void OnEnable()
    {
        CombatEvents.DamageDealt += NpcHit;
    }

    private void OnDisable()
    {
        CombatEvents.DamageDealt -= NpcHit;
    }


    private void Start()
    {
        _CanvasGroup = GetComponent<CanvasGroup>();
    }
    private void NpcHit(CombatCalculations.DamageResult result)
    {
        if (result.caster.tag == "Player" && result.target.tag != "Player")
        {
            StartCoroutine(FlashDragon());
        }
    }

    private IEnumerator FlashDragon()
    {
        _CanvasGroup.alpha = 1;
        yield return new WaitForSeconds(0.2f);
        _CanvasGroup.alpha = 0;
    }
}
