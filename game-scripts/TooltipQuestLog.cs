using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using BLINK.RPGBuilder.Managers;
using BLINK.RPGBuilder.UI;

[RequireComponent(typeof(TMP_Text))]
    public class TooltipQuestLog : MonoBehaviour, IPointerEnterHandler,IPointerExitHandler
    {
        private TMP_Text _tmpTextBox;
        public Canvas _canvasToCheck;
        [SerializeField] private Camera cameraToUse;
        public RPGItem curItem = null;
        public Transform DialogueToolTips;
        private Vector3 mousePosition;

    public delegate void ClickOnLinkEvent(string keyword);
        public static event ClickOnLinkEvent OnClickedOnLinkEvent;

        private void Awake()
        {
        cameraToUse = Camera.main;
        _tmpTextBox = GetComponent<TMP_Text>();
        _canvasToCheck = GetComponentInParent<Canvas>();

            if (_canvasToCheck.renderMode == RenderMode.ScreenSpaceOverlay)
                cameraToUse = null;
            else
            cameraToUse = _canvasToCheck.worldCamera;

           DialogueToolTips = GameObject.FindGameObjectWithTag("dialogue-tooltips").transform;
        }


        public void OnPointerEnter(PointerEventData eventData)
        {
            mousePosition = new Vector3(eventData.position.x, eventData.position.y, 0);
            var word = TMP_TextUtilities.FindIntersectingLink(_tmpTextBox, mousePosition, cameraToUse);

        if (word != -1)
            {
            Debug.Log("Link" + _tmpTextBox.textInfo.linkInfo[word].GetLinkText());
            curItem = GetItem(_tmpTextBox.textInfo.linkInfo[word].GetLinkText());
                ShowTooltip();
            }
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            HideTooltip();
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
            {
            DialogueToolTips.GetChild(1).GetComponent<ItemTooltip>().Show(curItem.ID, -1, false);
            DialogueToolTips.GetChild(1).GetComponent<CanvasGroup>().alpha = 1;
            }
        
        }

        public void HideTooltip()
        {
            ItemTooltip.Instance.Hide();
            DialogueToolTips.GetChild(1).GetComponent<CanvasGroup>().alpha = 0;
        }
  }