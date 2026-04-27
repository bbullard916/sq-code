using UnityEngine;
using UnityEngine.EventSystems;
using BLINK.RPGBuilder.UI;
public class ToolTipHandler : MonoBehaviour,  IPointerEnterHandler, IPointerExitHandler
{

    public int itemID;
    public void OnPointerEnter(PointerEventData eventData)
    {
        ShowTooltip();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HideTooltip();
    }
    public void ShowTooltip()
    {
        ItemTooltip.Instance.Show(itemID, -1,true);
    }

    public void HideTooltip()
    {
        ItemTooltip.Instance.Hide();
    }
}
