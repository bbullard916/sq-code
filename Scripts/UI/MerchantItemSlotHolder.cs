using BLINK.RPGBuilder.Managers;
using BLINK.RPGBuilder.UI;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BLINK.RPGBuilder.UIElements
{
    public class MerchantItemSlotHolder : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public Image itemIcon, background;
        public TextMeshProUGUI ItemNameText, ItemPriceText;

        public RPGItem thisItem;
        public RPGCurrency thisCurrency;
        public CanvasGroup outline;
        private Transform slotParent;
        public int thisCost;
        public  MerchantPanel merchantPanel;
        public bool isSelected = false;
        public void Init(RPGItem item, RPGCurrency currency, int cost)
        {
            slotParent = this.gameObject.transform.parent;
            thisItem = item;
            thisCurrency = currency;
            thisCost = cost;
            itemIcon.sprite = thisItem.entryIcon;
            background.sprite = thisItem.ItemRarity.background;
            ItemNameText.text = thisItem.entryDisplayName;
            merchantPanel = GameObject.Find("Merchant").GetComponent<MerchantPanel>();
            var costText = thisCost.ToString();
            isSelected = false;
            if (currency.convertToCurrencyID != -1)
            {
                var currencyREF = GameDatabase.Instance.GetCurrencies()[currency.convertToCurrencyID];
                if (currencyREF != null && thisCurrency.AmountToConvert > 0)
                {
                    if (thisCost >= currency.AmountToConvert)
                    {
                        var convertedCurrencyCount = thisCost / thisCurrency.AmountToConvert;
                        var remaining = thisCost % thisCurrency.AmountToConvert;
                        costText = convertedCurrencyCount + " " + currencyREF.entryDisplayName + " " + remaining + " " +
                                   thisCurrency.entryDisplayName;
                    }
                    else
                    {
                        costText = thisCost.ToString();
                    }
                }
                else
                {
                    costText = thisCost.ToString();
                }
            }


            ItemPriceText.text = costText;
        }


        public void BuyThisItem()
        {
            EconomyUtilities.BuyItemFromMerchant(thisItem, thisCurrency, thisCost);
            UIInfoWindow.Instance.SendGeneralMessage("You purchased: " + thisItem.displayName);
        }

        public void clearOutline()
        {
            merchantPanel.ClearMerchatOutline();
            isSelected = true;
        }
        public void SetOutline(bool value)
        {
            if (value)
            {
                outline.alpha = 1;
            }
            else
            {
                outline.alpha = 0;
            }
        }

        public void ShowTooltip()
        {
            ItemTooltip.Instance.Show(thisItem.ID, -1, false);
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
}