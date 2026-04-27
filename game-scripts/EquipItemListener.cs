using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipItemListener : MonoBehaviour
{
    public GameObject[] Items;
    public Transform Armor;
    private void OnEnable()
    {
       // GeneralEvents.PlayerEquippedItem += PlayerEquippedItem;
       // GeneralEvents.PlayerUnequippedItem += PlayerUnequippedItem;
    }

    private void OnDisable()
    {
        //GeneralEvents.PlayerEquippedItem -= PlayerEquippedItem;
       // GeneralEvents.PlayerUnequippedItem -= PlayerUnequippedItem;
    }

    private void PlayerEquippedItem(RPGItem itemEquipped)
    {
        Debug.Log("Equipped: " + itemEquipped.name);
        Armor = GameObject.Find("Armors").transform;
        foreach (GameObject item in Items)
        {
            if (item.name == itemEquipped.name.Replace("_ITEM",""))
            {
                Instantiate(item, Armor);
                Animator _Animator = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
                _Animator.Rebind();
                _Animator.Update(0f);
            }
        }
    }
    private void PlayerUnequippedItem(RPGItem itemEquipped)
    {
        Debug.Log("Unequipped: " + itemEquipped.name);
        Armor = GameObject.Find("Armors").transform;
        foreach (GameObject item in Items)
        {
            if (item.name == itemEquipped.name.Replace("_ITEM", ""))
            {
                DestroyImmediate(GameObject.Find(item.name+"(Clone)"));
            }
        }
    }
}
