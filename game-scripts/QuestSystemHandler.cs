using PixelCrushers.DialogueSystem.RPGBuilderSupport;
using PixelCrushers.QuestMachine.DialogueSystemSupport;
using PixelCrushers.QuestMachine;
using PixelCrushers;
using UnityEngine;


    public class QuestSystemHandler : MonoBehaviour
    {
        public QuestControl _QuestControl;
        public StringField[] QuestIDS;
        public StringField[] QuestCounters;
        private DialogueSystemRPGBuilderBridge RpgBuilderBridgeinstance;
        private DialogueSystemQuestMachineBridge QuestMachineBridgeinstance;

        public void SetQuestItemCollectedCounter(string QuestID)
        {
            if (QuestID == "Goblins Blood")
            {
                _QuestControl.questID = QuestIDS[0];
                _QuestControl.questID = QuestIDS[0];
                _QuestControl.IncrementQuestCounter(1);
                UIInfoWindow.Instance.SendGeneralMessage("x1 Vile of goblin blood collected");
                SimpleCustomAlrtUI.Instance.SendCustomMessage("<color=green>(Quest Item)</color> <color=#2BA0FAFF>x1 Vile of goblin blood collected </color>", "red");
        }
        }
    }
