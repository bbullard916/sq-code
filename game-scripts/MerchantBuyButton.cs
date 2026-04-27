using BLINK.RPGBuilder.UIElements;
using UnityEngine;
using BLINK.RPGBuilder.Managers;
public class MerchantBuyButton : MonoBehaviour
{
    public Transform merchantItems;
    public MerchantPanel merchantPanel;
    public void BuyCurrentSelectedItem()
    {
        for (int i = 0; i < merchantItems.childCount; i++)
        {
            MerchantItemSlotHolder item = merchantItems.GetChild(i).GetComponent<MerchantItemSlotHolder>();
            if ( item.isSelected)
            {
                item.BuyThisItem();
                merchantPanel.UpdateCurrency();
            }
        }
    }
}
