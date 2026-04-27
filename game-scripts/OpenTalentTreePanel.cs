using BLINK.RPGBuilder.Managers;
using UnityEngine;
using BLINK.RPGBuilder.Characters;

public class OpenTalentTreePanel : MonoBehaviour
{
    private RPGTalentTree thisTree;

    public void OpenTalenTree()
    {
        foreach (var t in GameDatabase.Instance.GetClasses()[Character.Instance.CharacterData.ClassID].talentTrees)
        {
            if (t.talentTreeID == 0)
            {
                InitSlot(GameDatabase.Instance.GetTalentTrees()[t.talentTreeID]);
            }
        }
    }
    public void InitSlot(RPGTalentTree cbtTree)
    {
        thisTree = cbtTree;
    }
    public void OpenTalentPanel()
    {
        UIEvents.Instance.OnSetPreviousTalentTreeMenu(TalentTreePreviousMenu.CharacterPanel);
        UIEvents.Instance.OnShowTalentTree(thisTree);

    }
}
