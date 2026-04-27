using UnityEngine;
using PixelCrushers;
using PixelCrushers.QuestMachine;
using MTAssets.EasyMinimapSystem;

public class QuestMiniMapListener : MonoBehaviour, IMessageHandler
{
    public string QuestNode;
    public GameObject[] mmItemGO;
    private void OnEnable()
    {
        MessageSystem.AddListener(this, QuestMachineMessages.QuestStateChangedMessage, "");
    }

    private void OnDisable()
    {
        MessageSystem.RemoveListener(this);
    }

    public void OnMessage(MessageArgs messageArgs)
    {
        // Message: "Quest State Changed"
        // - Parameter: Quest ID. 
        // - Argument 0: [StringField] Quest node ID, or null for main quest state.
        // - Argument 1: [QuestState] / [QuestNodeState] New state.
        var questID = messageArgs.parameter;
        if (messageArgs.values[0] == null)
        {
            var questState = (QuestState)messageArgs.values[1];
            //Debug.Log($"Quest {questID} changed to state {questState}");
            if (questState.ToString() == "WaitingToStart")
            {
                foreach (GameObject obj in mmItemGO)
                {
                    obj.SetActive(false);
                }
            }
        }
        else
        {
            var questNodeID = (StringField)messageArgs.values[0];
            var questNodeState = (QuestNodeState)messageArgs.values[1];
            Debug.Log($"Quest {questID} node {questNodeID} changed to state {questNodeState}");
            if (questNodeID.ToString() == QuestNode)
            {
                if (questNodeState.ToString() == "Active")
                {
                    foreach(GameObject obj in mmItemGO)
                    {
                        obj.SetActive(true);
                    }
                }
                else if(questNodeState.ToString() == "True")
                {
                    foreach (GameObject obj in mmItemGO)
                    {
                        obj.SetActive(false);
                    }
                }
                else
                {
                    foreach (GameObject obj in mmItemGO)
                    {
                        obj.SetActive(false);
                    }
                }
            }
        }
    }
}