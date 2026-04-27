using TMPro;
using UnityEngine;
using UnityEngine.UI;
using PixelCrushers.DialogueSystem;
using System.Collections.Generic;
namespace BLINK.RPGBuilder.Managers
{
    public class CustomQuestJournalPanel : QuestLogWindow
    {
        [SerializeField] private CanvasGroup thisCG;
        [SerializeField] private Transform ObjectivesGroup;
        [SerializeField] private Transform RewardSlotsGrid;
        [SerializeField] private string CurrentTab;
        [SerializeField] public GameObject QuestButtonPrefab;
        [SerializeField] public GameObject QuestIcon;
        [SerializeField] public GameObject RewardsHeaderText;
        [SerializeField] public GameObject ObjectivesHeaderText;
        [SerializeField] public GameObject QuestRequirementPrefab;
        [SerializeField] private Transform ActiveQuestContent, CompletedQuestContent;
        [SerializeField] private string CurrentPanelSelected;
        [SerializeField] private TextMeshProUGUI questNameText, descriptionText;
        [SerializeField] public Button abandonQuest, trackQuest;
        [SerializeField] public GameObject RewardSlot;
        [SerializeField] public GameObject ObjectiveItem;
        [SerializeField] public GameObject TrackQuestButton;
        [SerializeField] public GameObject AbandonQuestButton;
        [SerializeField] public GameObject ExperienceGo;
        [SerializeField] public TextMeshProUGUI ExperienceAmountText;
        [SerializeField] private string CurrentSelectedQuest;
        private bool m_isAwake = false;
        [Tooltip("Add an EventSystem if one isn't in the scene.")]
        public bool addEventSystemIfNeeded = true;

        [SerializeField] private GameObject questItemSlotPrefab, objectiveTextPrefab, questStateSlotPrefab;


        #region Runtime Properties

        private StandardUIInstancedContentManager m_selectionPanelContentManager = new StandardUIInstancedContentManager();
        protected StandardUIInstancedContentManager selectionPanelContentManager
        {
            get { return m_selectionPanelContentManager; }
            set { m_selectionPanelContentManager = value; }
        }

        private StandardUIInstancedContentManager m_detailsPanelContentManager = new StandardUIInstancedContentManager();
        protected StandardUIInstancedContentManager detailsPanelContentManager
        {
            get { return m_detailsPanelContentManager; }
            set { m_detailsPanelContentManager = value; }
        }

        protected List<string> expandedGroupNames = new List<string>();
        protected System.Action confirmAbandonQuestHandler = null;
        private Coroutine m_refreshCoroutine = null;

        #endregion
        private void OnEnable()
        {

            if (GameState.IsInGame())
            {
                if (UIEvents.Instance.IsPanelOpen("Quest Log")) Show();
            }
        }

        public override void Awake()
        {
            m_isAwake = true;
            base.Awake();
            if (addEventSystemIfNeeded) UITools.RequireEventSystem();
        }


        public  void Show()
        {
            ExperienceGo.SetActive(false);
            RewardsHeaderText.SetActive(false);
            QuestIcon.SetActive(false);
            ObjectivesHeaderText.SetActive(false);
            AbandonQuestButton.SetActive(false);
            TrackQuestButton.SetActive(false);
            RPGBuilderUtilities.EnableCG(thisCG);
            transform.SetAsLastSibling();
            CustomInputManager.Instance.AddOpenedPanel(thisCG);
            InitActiveQuests();
        }


        public void Hide()
        {
            gameObject.transform.SetAsFirstSibling();
            RPGBuilderUtilities.DisableCG(thisCG);
            if (CustomInputManager.Instance != null) CustomInputManager.Instance.HandleUIPanelClose(thisCG);
        }

        public void ClearQuestContent()
        {
            descriptionText.text = "";
            questNameText.text = "";

            foreach (Transform child in ActiveQuestContent.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
            foreach (Transform child in CompletedQuestContent.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
        }

        public void ClearQuestDetails()
        {
            foreach (Transform child in ObjectivesGroup)
            {
                GameObject.Destroy(child.gameObject);
            }
            foreach (Transform child in RewardSlotsGrid)
            {
                GameObject.Destroy(child.gameObject);
            }

        }

        public void ClearQuestObjectives()
        {
            foreach (Transform child in ObjectivesGroup)
            {
                if (child.gameObject.name.Contains("Clone"))
                    GameObject.Destroy(child.gameObject);
            }
        }
        public void InitActiveQuests()
        {
            ClearQuestObjectives();
            ClearQuestContent();
            ClearQuestRewards();
            CurrentPanelSelected = "Active";
            string[] quests = QuestLog.GetAllQuests();
            Debug.Log("Showing Active Quests");
            if (quests.Length > 0)
            {
                RewardsHeaderText.SetActive(true);
                QuestIcon.SetActive(true);
                ObjectivesHeaderText.SetActive(true);
                for (int i = 0; i < quests.Length; i++)
                {
                    if (i == 0)
                    {
                        string QuestName = QuestLog.GetQuestTitle(quests[i]);
                        InitQuestRewards(quests[i]);
                        GameObject QuestButton = Instantiate(QuestButtonPrefab, ActiveQuestContent);
                        QuestButton.transform.Find("QuestName").GetComponent<TextMeshProUGUI>().text = QuestName;
                        DisplayQuestDetails(quests[i]);
                        for (int j = 0; j < QuestLog.GetQuestEntryCount(QuestName); j++)
                        {
                            var entryState = QuestLog.GetQuestEntryState(QuestName, j+1);
                            Debug.Log("QUESTSTATE: " + entryState);
                            var entryText = FormattedText.Parse(GetQuestEntryText(QuestName, j+1, entryState), DialogueManager.masterDatabase.emphasisSettings).text;
                            var entryText2 = GetQuestEntryText(QuestName, j + 1, entryState);
                            Debug.Log("ENTRY: "+ entryText);
                            Debug.Log("ENTRY2: " + entryText2);
                            if (entryState == QuestState.Active)
                            {
                                GameObject Objective = Instantiate(ObjectiveItem, ObjectivesGroup);
                                Objective.SetActive(true);
                                Objective.transform.Find("Amount Group/Value").GetComponent<TextMeshProUGUI>().text = entryText;
                            }
                        }
                        if (QuestLog.IsQuestTrackingEnabled(quests[i]))
                        {
                            TrackQuestButton.SetActive(true);
                            TrackQuestButton.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = "Untrack";
                        }
                        else
                        {
                            TrackQuestButton.SetActive(true);
                            TrackQuestButton.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = "Track";
                        }
                        if (QuestLog.IsQuestAbandonable(quests[i]))
                        {
                            AbandonQuestButton.SetActive(true);
                        }
                        else
                        {
                            AbandonQuestButton.SetActive(false);
                        }    
                    }
                    else
                    {
                        string QuestName = QuestLog.GetQuestTitle(quests[i]);
                        GameObject QuestButton = Instantiate(QuestButtonPrefab, ActiveQuestContent);
                        QuestButton.transform.Find("QuestName").GetComponent<TextMeshProUGUI>().text = QuestName;
                        QuestButton.transform.Find("Active Overlay").gameObject.SetActive(false);
                    }
                }
            }
        }

        private void setCurrenQuest(string Quest)
        {
            CurrentSelectedQuest = Quest;
        }

        public void InitCompletedQuests()
        {
            RewardsHeaderText.SetActive(false);
            QuestIcon.SetActive(false);
            ObjectivesHeaderText.SetActive(false);
            ClearQuestObjectives();
            ClearQuestContent();
            CurrentPanelSelected = "Completed";
            QuestGroupRecord[] quests = QuestLog.GetAllGroupsAndQuests(QuestState.Success, true);
            Debug.Log("Showing Active Quests");
            if (quests.Length > 0)
            {
                RewardsHeaderText.SetActive(true);
                QuestIcon.SetActive(true);
                ObjectivesHeaderText.SetActive(true);
                for (int i = 0; i < quests.Length; i++)
                {
                    if (i == 0)
                    {
                        string QuestName = QuestLog.GetQuestTitle(quests[i].questTitle);
                        GameObject QuestButton = Instantiate(QuestButtonPrefab, CompletedQuestContent);
                        QuestButton.transform.Find("QuestName").GetComponent<TextMeshProUGUI>().text = QuestName;
                        DisplayQuestDetails(quests[i].questTitle);
                        for (int j = 0; j < QuestLog.GetQuestEntryCount(QuestName); j++)
                        {
                            var entryState = QuestLog.GetQuestEntryState(QuestName, j + 1);
                            var entryText = FormattedText.Parse(GetQuestEntryText(QuestName, j + 1, entryState), DialogueManager.masterDatabase.emphasisSettings).text;
                            Debug.Log("ENTRY: " + entryText);
                            if (entryState == QuestState.Success)
                            {
                                GameObject Objective = Instantiate(ObjectiveItem, ObjectivesGroup);
                                Objective.SetActive(true);
                                Objective.transform.Find("Amount Group/Value").GetComponent<TextMeshProUGUI>().text = entryText;
                            }
                        }
                    }
                    else
                    {
                        string QuestName = QuestLog.GetQuestTitle(quests[i].questTitle);
                        GameObject QuestButton = Instantiate(QuestButtonPrefab, ActiveQuestContent);
                        QuestButton.transform.Find("QuestName").GetComponent<TextMeshProUGUI>().text = QuestName;
                        QuestButton.transform.Find("Active Overlay").gameObject.SetActive(false);
                    }
                }
            }
        }

        public void DisplayQuestDetails(string quest)
        {
            setCurrenQuest(quest);
            Debug.Log("Quest Details: " + QuestLog.GetQuestTitle(quest));
            questNameText.text = QuestLog.GetQuestTitle(quest);
            descriptionText.text = QuestLog.GetQuestDescription(quest);
            var items = DialogueManager.masterDatabase.items.FindAll(item => item.IsItem);
            foreach (var item in items)
            {
                for (int i = 0; i < item.fields.Count; i++)
                {
                    if (item.fields[i].title == "QuestReward")
                    {
                        Debug.Log("Reward Quest " + item.fields[i].value);
                    }
                }
            }
        }


        public void HandleClickQuest(string Questname)
        {
            ExperienceGo.SetActive(false);
            if (CurrentPanelSelected == "Active")
            {
                ClearQuestObjectives();
                ClearQuestRewards();
                for (int i = 0; i < ActiveQuestContent.childCount; i++)
                {
                    if (ActiveQuestContent.transform.GetChild(i).transform.Find("QuestName").GetComponent<TextMeshProUGUI>().text == Questname)
                    {
                        ActiveQuestContent.transform.GetChild(i).transform.Find("Active Overlay").gameObject.SetActive(true);
                    }
                    else if (ActiveQuestContent.transform.GetChild(i).transform.Find("QuestName").GetComponent<TextMeshProUGUI>().text != Questname)
                    {
                        ActiveQuestContent.transform.GetChild(i).transform.Find("Active Overlay").gameObject.SetActive(false);
                    }
                }
                DisplayQuestDetails(Questname);
                InitQuestRewards(Questname);
                for (int j = 0; j < QuestLog.GetQuestEntryCount(Questname); j++)
                {
                    var entryState = QuestLog.GetQuestEntryState(Questname, j + 1);
                    var entryText = FormattedText.Parse(GetQuestEntryText(Questname, j + 1, entryState), DialogueManager.masterDatabase.emphasisSettings).text;
                    Debug.Log("ENTRY: " + entryText);
                    GameObject Objective = Instantiate(ObjectiveItem, ObjectivesGroup);
                    Objective.SetActive(true);
                    Objective.transform.Find("Amount Group/Value").GetComponent<TextMeshProUGUI>().text = entryText;
                }
                if (QuestLog.IsQuestTrackingEnabled(Questname))
                {
                    TrackQuestButton.SetActive(true);
                    TrackQuestButton.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = "Untrack";
                }
                else
                {
                    TrackQuestButton.SetActive(true);
                    TrackQuestButton.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = "Track";
                }
                if (QuestLog.IsQuestAbandonable(Questname))
                {
                    AbandonQuestButton.SetActive(true);
                }
                else
                {
                    AbandonQuestButton.SetActive(false);
                }
            }
            else if (CurrentPanelSelected == "Completed")
            {

            }
        }

        public void InitQuestRewards(string Quest)
        {
            
            int Count = DialogueLua.GetQuestField(Quest, "RewardCount").asInt;
            Debug.Log("Count" + Count);
            if (Count > 0)
            {
                for (int i = 0; i < Count; i++)
                {
                    string ItemName = DialogueLua.GetQuestField(Quest, "Reward"+i).asString;
                    if (ItemName.Length>0)
                    {
                        Debug.Log("Rewards" + i+1);
                        Debug.Log("Item Returned: " + ItemName);
                        int itemID = GetItemID(ItemName);
                        RPGItem newItemREF = GameDatabase.Instance.GetItems()[itemID];
                        Debug.Log("Sprite: " + newItemREF.entryIcon.name);
                        GameObject QuestReward = Instantiate(RewardSlot, RewardSlotsGrid);
                        QuestReward.transform.Find("Icon").GetComponent<Image>().sprite = newItemREF.entryIcon;
                        QuestReward.SetActive(true);
                        QuestReward.transform.GetComponent<ToolTipHandler>().itemID = itemID;
                    }
                }
            }
            else
            {
                Debug.Log("No Rewards");
            }
            int QuestExperience = DialogueLua.GetQuestField(Quest, "Experience").asInt;
            if (QuestExperience >0)
            {
                ExperienceGo.SetActive(true);
                ExperienceAmountText.text = QuestExperience.ToString();
            }
           var itemToAdd = GameDatabase.Instance.GetItems()[GetItemID("Torch")];
        }

        public void ClearQuestRewards()
        {
            foreach (Transform child in RewardSlotsGrid)
            {
                GameObject.Destroy(child.gameObject);
            }
        }

        public void RewardShowTooltip()
        {
            //ItemTooltip.Instance.Show(thisItem.ID, Character.Instance.CharacterData.Inventory.baseSlots[slotIndex].itemDataID, true);
        }

        public void RewardHideTooltip()
        {
            //ItemTooltip.Instance.Hide();
        }
        private int GetItemID(string itemName)
        {
            foreach (RPGItem item in GameDatabase.Instance.GetItems().Values)
            {
                if (item != null && string.Equals(item.entryName, itemName))
                {
                    return item.ID;
                }
            }
            Debug.LogWarning($"Dialogue System: Can't find RPG Builder item named '{itemName}'");
            return -1;
        }
        public void HandleAbandonQuest()
        {
            if (CurrentPanelSelected == "Active")
            {
                QuestLog.AbandonQuest(CurrentSelectedQuest);
                ClearQuestContent();
                InitActiveQuests();
            }
        }

        public void HandleTrackQuest()
        {
            if (QuestLog.IsQuestTrackingEnabled(CurrentSelectedQuest))
            {
                QuestLog.SetQuestTracking(CurrentSelectedQuest, false);
                TrackQuestButton.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = "Untrack";
            }
            else
            {
                QuestLog.SetQuestTracking(CurrentSelectedQuest, true);
            TrackQuestButton.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = "Track";
            }
            if (TrackQuestButton.transform.Find("Text").GetComponent<TextMeshProUGUI>().text == "Track")
            {
                TrackQuestButton.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = "Untrack";
            }
            else
            {
                TrackQuestButton.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = "Track";
            }
        }

        private string GetQuestEntryText(string quest, int entryNum, QuestState entryState)
        {
            if (entryState == QuestState.Unassigned || entryState == QuestState.Abandoned)
            {
                return string.Empty;
            }
            else if (entryState == QuestState.Success)
            {
                var text = DialogueLua.GetQuestField(quest, "Entry " + entryNum + " Success").asString;
                Debug.Log("QUESTFIELD" + text);
                if (!string.IsNullOrEmpty(text)) return text;
            }
            else if (entryState == QuestState.Failure)
            {
                var text = DialogueLua.GetQuestField(quest, "Entry " + entryNum + " Failure").asString;
                if (!string.IsNullOrEmpty(text)) return text;
            }
            return QuestLog.GetQuestEntry(quest, entryNum);
        }
    }
}