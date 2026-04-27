using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
public class CharacterCreateHoverStats : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI DescriptionText;

    public void Start()
    {
        DescriptionText = GameObject.Find("Stat Description").GetComponent<TextMeshProUGUI>();
    }
    public  void OnPointerEnter(PointerEventData eventData)
    {
        DescriptionText.text =  this.gameObject.GetComponent<TextMeshProUGUI>().text;
        if(DescriptionText.text == "Strength")
        {
            DescriptionText.text = "<color=#2BA0FAFF>Strength:</color> <color=white>The amount of extra physical damage you can do 0.1 dmg per additonal point</color>";
        }
        else if (DescriptionText.text == "Intellect")
        {
            DescriptionText.text = "<color=#2BA0FAFF>Intellect:</color> <color=white>The amount of extra damage dealt by all Magical abilities and general intellect</color>";
        }
        else if (DescriptionText.text == "Constitution")
        {
            DescriptionText.text = "<color=#2BA0FAFF>Constitution: <color=white>The amount of HP you gain on each level upt</color>";
        }
        else if (DescriptionText.text == "Fortitude")
        {
            DescriptionText.text = "<color=#2BA0FAFF>Fortitude: <color=white>Your general toughness which adds bounus to your Armor Class</color>";
        }
        else if (DescriptionText.text == "Agility")
        {
            DescriptionText.text = "<color=#2BA0FAFF>Agility:</color> <color=white>Your ability to doge and avoid damage, this also attributes to your critical hit chance";
        }
        else if (DescriptionText.text == "Charisma")
        {
            DescriptionText.text = "<color=#2BA0FAFF>Charisma:</color> <color=white>How other react to you and opens up new dialogue options";
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        DescriptionText.text = "";
    }
}
