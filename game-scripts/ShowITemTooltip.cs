using BLINK.RPGBuilder.UI;
using UnityEngine;
using UnityEngine.EventSystems;
public class ShowITemTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public int ItemID;
    public void ShowTooltip()
    {
        ItemTooltip.Instance.Show(ItemID, -1, false);
    }

    public void HideTooltip()
    {
        ItemTooltip.Instance.Hide();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ShowTooltip();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HideTooltip();
    }
}
