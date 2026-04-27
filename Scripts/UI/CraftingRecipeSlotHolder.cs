using BLINK.RPGBuilder.Managers;
using BLINK.RPGBuilder.UI;
using BLINK.RPGBuilder.UIElements;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BLINK.RPGBuilder.UIElements
{
    public class CraftingRecipeSlotHolder : MonoBehaviour
    {
        public Image icon, background;
        public TextMeshProUGUI nameText, statusText, countText;
        public RPGCraftingRecipe thisRecipe;
        public CanvasGroup craftButton;
        public CanvasGroup craftingBar;
        public CanvasGroup outline;

        public void InitSlot(RPGCraftingRecipe recipe)
        {
            icon.sprite = recipe.entryIcon;
            background.sprite = GameDatabase.Instance.GetItems()[recipe.ranks[RPGBuilderUtilities.getRecipeRank(recipe.ID)].allCraftedItems[0]
                    .craftedItemID].ItemRarity.background;
            nameText.text = recipe.entryDisplayName;
            thisRecipe = recipe;
            craftButton = GameObject.FindGameObjectWithTag("Craft Button").GetComponent<CanvasGroup>();
            craftingBar = GameObject.Find("craftingBar").GetComponent<CanvasGroup>();
        }

        public void UpdateState(string status, int count)
        {
            statusText.text = status;
            countText.text = count.ToString();
        }

        public void SetOutline(bool isOutline)
        {
            if (isOutline)
            {
                ClearOutline();
                outline.alpha = 1;
            }
            else
            {
                outline.alpha = 0;
            }
        }

        private void ClearOutline()
        {
            for (int i = 0; i < this.gameObject.transform.parent.childCount; i++)
            {
                this.gameObject.transform.parent.GetChild(i).GetComponent<CraftingRecipeSlotHolder>().SetOutline(false);
            }
        }
        public void SelectRecipe()
        {
            UIEvents.Instance.OnDisplayCraftingRecipeInPanel(thisRecipe);
            if (statusText.text.Contains("Missing"))
            {
                craftButton.alpha = 0;
                craftButton.interactable = false;
                craftingBar.alpha = 0;
                craftingBar.interactable = false;
            }
            else
            {
                craftButton.alpha = 1;
                craftButton.interactable = true;
                craftingBar.alpha = 1;
                craftingBar.interactable = true;  
            }
        }

        public void ShowTooltip()
        {
            var curRank = RPGBuilderUtilities.getRecipeRank(thisRecipe.ID);
            var rankREF = thisRecipe.ranks[curRank];
            ItemTooltip.Instance.Show(rankREF.allCraftedItems[0].craftedItemID, -1, false);
        }

        public void HideTooltip()
        {
            ItemTooltip.Instance.Hide();
        }
    }
}