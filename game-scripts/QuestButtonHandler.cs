using UnityEngine;
using BLINK.RPGBuilder.Managers;
using TMPro;
public class QuestButtonHandler : MonoBehaviour
{
    CustomQuestJournalPanel _CustomQuestJournalPanel;
    string Questname;
    void Start()
    {
        _CustomQuestJournalPanel = GameObject.FindGameObjectWithTag("Quest Log Panel").GetComponent<CustomQuestJournalPanel>();
    }

    public void HandleClick()
    {
        Questname = this.gameObject.transform.Find("QuestName").GetComponent<TextMeshProUGUI>().text;
        _CustomQuestJournalPanel.HandleClickQuest(Questname);
    }
}
