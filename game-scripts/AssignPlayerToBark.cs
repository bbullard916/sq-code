using System.Collections;
using UnityEngine;
using PixelCrushers.DialogueSystem;

public class AssignPlayerToBark : MonoBehaviour
{
    IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();
        var trigger = GetComponent<DialogueSystemTrigger>();
        if (trigger == null) yield break;
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) yield break;
        trigger.barker = player.transform;
    }
}
