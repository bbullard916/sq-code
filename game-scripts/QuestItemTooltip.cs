using System.Collections.Generic;
using BLINK.RPGBuilder.Characters;
using BLINK.RPGBuilder.Data;
using BLINK.RPGBuilder.Managers;
using BLINK.RPGBuilder.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

namespace BLINK.RPGBuilder.DisplayHandler
{
    public class QuestItemTooltip : MonoBehaviour
    {
        public RPGItem curItem;
        //public Text ItemNameText;
        public TMP_Text ItemNameText;
        private int itemDataID = -1;

        public void Start()
        {
            curItem = GetItem(ItemNameText.text);
        }
        public virtual double rpgGetItemAmount(string itemName)
        {
            int itemID = GetItemID(itemName);
            if (itemID == -1) return 0;
            return EconomyUtilities.GetTotalItemCount(itemID);
        }
        protected virtual int GetItemID(string itemName)
        {
            var item = GetItem(itemName);
            return (item != null) ? item.ID : -1;
        }
        protected virtual RPGItem GetItem(string itemName)
        {
            foreach (RPGItem item in GameDatabase.Instance.GetItems().Values)
            {
                if (item != null && string.Equals(item.entryName, itemName))
                {
                    return item;
                }
            }
            Debug.LogWarning($"Dialogue System: Can't find RPG Builder item named '{itemName}'");
            return null;
        }
        public void ShowTooltip()
        {
            if (curItem != null)
                ItemTooltip.Instance.Show(curItem.ID, -1, false);
        }

        public void HideTooltip()
        {
            ItemTooltip.Instance.Hide();
        }

        //public void OnPointerEnter(PointerEventData eventData)
     //   {
      //      ShowTooltip();
     //   }
      //  public void OnPointerExit(PointerEventData eventData)
     //   {
      //      Debug.Log("Hiding Tooltip");
      //      HideTooltip();
     //   }
    }
}
