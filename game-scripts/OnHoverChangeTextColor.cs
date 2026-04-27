using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class OnHoverChangeTextColor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public TextMeshProUGUI Text;
    public Color TextColorEnter;
    public Color TextColorExit;
    public void OnPointerEnter(PointerEventData eventData)
    {
        Text.color = TextColorEnter; //Or however you do your color
        SoundManager.Instance.PlayUiClick(0);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Text.color = TextColorExit; //Or however you do your color
    }
}