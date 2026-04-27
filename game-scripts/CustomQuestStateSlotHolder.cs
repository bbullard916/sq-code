using BLINK.RPGBuilder.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BLINK.RPGBuilder.UIElements
{
    public class CustomQuestStateSlotHolder : MonoBehaviour
    {
        public enum QuestSlotPanelType
        {
            interactionPanel,
            questJournal
        }

        private QuestSlotPanelType panelType;

        public Image icon, background;
        public TextMeshProUGUI questNameText;
        private RPGQuest thisQuest;
        public CustomQuestJournalPanel _CustomQuestJournalPanel;

        public void InitSlot(RPGQuest quest, Color bgColor, Sprite stateIcon, QuestSlotPanelType _type)
        {
            panelType = _type;
            icon.sprite = stateIcon;
            questNameText.text = quest.entryDisplayName;
            background.color = bgColor;
            thisQuest = quest;
            _CustomQuestJournalPanel = GameObject.Find("Quest Log").GetComponent<CustomQuestJournalPanel>();
        }

        public void ClickQuest()
        {
            transform.GetChild(2).GetComponent<UnityEngine.UI.Image>().enabled = true;
            if (panelType == QuestSlotPanelType.interactionPanel)
            {
                UIEvents.Instance.OnDisplayQuest(thisQuest, true);
            }

            else
            {
                UIEvents.Instance.OnDisplayQuestInJournal(thisQuest);
            }
        }
    }
}