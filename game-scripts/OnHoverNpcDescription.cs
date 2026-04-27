using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using PixelCrushers.DialogueSystem;

public class OnHoverNpcDescription : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TMP_Text Description;
    public StandardUISubtitlePanel _Panel;
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData != null)
        {
            Description.text = PixelCrushers.DialogueSystem.CharacterInfo.GetLocalizedDisplayNameInDatabase(DialogueLua.GetActorField(_Panel.portraitActorName, "Description").asString);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData != null)
            Description.text = "";
    }
}
