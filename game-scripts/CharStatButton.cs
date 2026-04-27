using UnityEngine;
using UnityEngine.UI;
using BLINK.RPGBuilder.Managers;
public class CharStatButton : MonoBehaviour
{
    public CanvasGroup[] ButtonCanvas;
    public CanvasGroup StatsCanvas;
    public CanvasGroup HoverStats;
    public CustomCharacterPanel _CustomCharacterPanel;

    public void Start()
    {
        _CustomCharacterPanel.ClearStatText();
        StatsCanvas.interactable = false;
        StatsCanvas.alpha = 0;
        HoverStats.alpha = 0;
        ButtonCanvas[0].interactable = true;
        ButtonCanvas[0].blocksRaycasts = true;
        ButtonCanvas[0].alpha = 1;
        ButtonCanvas[1].interactable = false;
        ButtonCanvas[1].alpha = 0;
        ButtonCanvas[1].blocksRaycasts = false;
    }

    public void EnableStatsPanel()
    {
        _CustomCharacterPanel.InitCharStats();
        StatsCanvas.interactable = true;
        StatsCanvas.alpha = 1;
        HoverStats.alpha = 1;
        ButtonCanvas[0].interactable = false;
        ButtonCanvas[0].alpha = 0;
        ButtonCanvas[0].blocksRaycasts = false;
        ButtonCanvas[1].interactable = true;
        ButtonCanvas[1].alpha = 1;
        ButtonCanvas[1].blocksRaycasts = true;
    }
    public void DisableStatsPanel()
    {
        HoverStats.alpha = 0;
        _CustomCharacterPanel.ClearStatText();
        StatsCanvas.interactable = false;
        StatsCanvas.alpha = 0;
        ButtonCanvas[0].interactable = true;
        ButtonCanvas[0].alpha = 1;
        ButtonCanvas[0].blocksRaycasts = true;
        ButtonCanvas[1].interactable = false;
        ButtonCanvas[1].alpha = 0;
        ButtonCanvas[1].blocksRaycasts = false;
    }

}
