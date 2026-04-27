using UnityEngine.UI;
using UnityEngine;


public class HandleOffhnadSlot : MonoBehaviour
{
    public Sprite[] OffhandWeaponImagesBW;
    public Sprite[] OffhandWeaponImagesColor;
    public Image Icon;
    void OnEnable()
    {
        GeneralEvents.PlayerEquippedItem += PlayerEquippedItem;
        GeneralEvents.PlayerUnequippedItem += PlayerUnequippedItem;
    }
    void OnDisable()
    {
        GeneralEvents.PlayerEquippedItem -= PlayerEquippedItem;
        GeneralEvents.PlayerUnequippedItem -= PlayerUnequippedItem;
    }


    private void PlayerEquippedItem(RPGItem itemEquipped)
    {
        if (itemEquipped.entryName.Contains("bow"))
        {
            Icon.sprite = OffhandWeaponImagesColor[0];
        }
    }

    private void PlayerUnequippedItem(RPGItem itemUnEquipped)
    {
        if (itemUnEquipped.entryName.Contains("bow"))
        {
            Icon.sprite = OffhandWeaponImagesBW[0];
        }
    }
}
