using BLINK.RPGBuilder.Characters;
using TMPro;
using UnityEngine;
using PixelCrushers.DialogueSystem;
public class UIInfoWindow : MonoBehaviour
{
    public Transform InfoText;
    public Transform CombatInfoWindowParent;
    public Transform DialogueInfoWindowParent;
    public Transform QuestInfoWindowParent;
    public TextMeshProUGUI AllText;
    public TextMeshProUGUI CombatText;
    public TextMeshProUGUI DialogueText;
    public TextMeshProUGUI QuestText;
    public static UIInfoWindow Instance { get; private set; }
    private void OnEnable()
    {

        CombatEvents.DamageDealt += ShowDamageText;
        CombatEvents.Healed += ShowHealText;
     //   CombatEvents.PlayerFactionPointChanged += ShowFactionPointText;
        GameEvents.CharacterExperienceChanged += ShowCharacterExperienceText;
    //    GameEvents.CharacterLevelChanged += ShowCharacterLevelUpText;
   //     GameEvents.WeaponTemplateExperienceChanged += ShowWeaponTemplateExperienceText;
    //    GameEvents.WeaponTemplateLevelChanged += ShowWeaponTemplateLevelUpText;
    //    GameEvents.SkillExperienceChanged += ShowSkillExperienceText;
    //    GameEvents.SkillLevelChanged += ShowSkillLevelUpText;
    }

    private void Awake()
    {
        if (Instance != null) return;
        Instance = this;
    }
    private void OnDisable()
    {
        CombatEvents.DamageDealt -= ShowDamageText;
        CombatEvents.Healed -= ShowHealText;
     //   CombatEvents.PlayerFactionPointChanged -= ShowFactionPointText;
        GameEvents.CharacterExperienceChanged -= ShowCharacterExperienceText;
    //    GameEvents.CharacterLevelChanged -= ShowCharacterLevelUpText;
   //     GameEvents.WeaponTemplateExperienceChanged -= ShowWeaponTemplateExperienceText;
    //    GameEvents.WeaponTemplateLevelChanged -= ShowWeaponTemplateLevelUpText;
    //    GameEvents.SkillExperienceChanged -= ShowSkillExperienceText;
    //    GameEvents.SkillLevelChanged -= ShowSkillLevelUpText;
    }

    private void ClearAllText()
    {
        foreach (Transform child in CombatInfoWindowParent.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }
    private void ShowCharacterExperienceText(int amount)
    {
        SendPlayerExperience(amount);
    }

    public void ShowDialogueText(string text)
    {
        string CurrentText = AllText.text;
        AllText.SetText("<i>" + text + "</i>" + "<br>" + CurrentText);
        DialogueText.SetText("<i>" + text + "</i>" + "<br>" + CurrentText);
    }
    private void ShowDamageText(CombatCalculations.DamageResult result)
    {

        if (!result.caster.IsPlayer() && !result.target.IsPlayer() && (!result.caster.IsPet() || !result.caster.GetOwnerEntity().IsPlayer())) return;
        string message = "";
        switch (result.DamageActionType)
        {
            case "Physical" when result.target == GameState.playerEntity:
            case "Magical" when result.target == GameState.playerEntity:
            case "Physical_CRITICAL" when result.target == GameState.playerEntity:
            case "Magical_CRITICAL" when result.target == GameState.playerEntity:
            case "Neutral" when result.target == GameState.playerEntity:
                message = "" + (int)result.DamageAmount;
                break;
            case "Physical":
            case "Magical":
            case "Physical_CRITICAL":
            case "Magical_CRITICAL":
            case "Neutral":
                message = "" + (int)result.DamageAmount;
                break;
            case "THORN" when result.target.IsPlayer():
            case "THORN":
                message = "" + result.DamageAmount;
                break;
            case "BLOCKED" when result.target.IsPlayer():
            case "BLOCKED":
                message = result.DamageAmount + " (Blocked " +
                          (result.DamageBlockedActively + result.DamageBlockedPassively) + ")";
                break;
            case "DODGED" when result.target.IsPlayer():
            case "DODGED":
                message = "Dodged";
                break;
        }

        ScreenEventHandler(result.DamageActionType, message, result.target.gameObject);
    }
    private void ShowHealText(CombatCalculations.HealResult result)
    {
        string message = "";
        switch (result.HealActionType)
        {
            case "HEAL" when result.target.IsPlayer():
            case "HEAL_CRITICAL" when result.target.IsPlayer():
                message = "" + (int)result.HealAmount;
                break;
            case "HEAL":
            case "HEAL_CRITICAL":
                message = "" + (int)result.HealAmount;
                break;
        }

        ScreenEventHandler(result.HealActionType, message, result.target.gameObject);
    }

    private void SendPlayerExperience(int data)
    {
        //Transform go = Instantiate(InfoText);
        //go.SetParent(CombatInfoWindowParent);
        // go.SetAsFirstSibling();
        // go.GetComponent<TextMeshProUGUI>().text = "";
        string CurrentText = AllText.text;
        AllText.text = "<color=blue>" + Character.Instance.CharacterData.CharacterName + "</color>" + " has gained " + "<color=green>" + data + "</color>" + " points of Experience"+ CurrentText + "<br>";
        CombatText.text = "<color=blue>" + Character.Instance.CharacterData.CharacterName + "</color>" + " has gained " + "<color=green>" + data + "</color>" + " points of Experience" + CurrentText + "<br>";
    }

    public void SendGeneralMessage(string text)
    {
        //Debug.Log("Adding Text" + text);
        string CurrentText = AllText.text;
        if (text.Contains("Quest Reward") || text.Contains("Quest Accepted"))
            QuestText.SetText("<color=green> <i>" + text + "</i>" + "<br></color>" + CurrentText);

        AllText.SetText("<color=green> <i>" + text + "</i>" + "<br></color>" + CurrentText);
    }
    private void ScreenEventHandler(string eventType, string message, GameObject target)
    {
        //Debug.Log("EventTYPE" + eventType);
        //Transform go = Instantiate(InfoText);
        //go.SetParent(CombatInfoWindowParent);
        // go.SetAsFirstSibling();
        // go.GetComponent<TextMeshProUGUI>().text = "";
        string CurrentText = AllText.text;
        string Name = "";
        if (target.tag == "Player" && target != null)
        {
            Name = Character.Instance.CharacterData.CharacterName;
        }
        else
        {
            Name = target.name;
        }

        if (eventType == "Physical" || eventType == "Magical" || eventType == "Neutral")
        {
            AllText.text = "<color=#ce4627ff>" + Name + "</color>" + " is hit taking  " + "<color=red>" + message + "</color>" + " points of Physical damage<br>"+ CurrentText;
            CombatText.text = "<color=#ce4627ff>" + Name + "</color>" + " is hit taking  " + "<color=red>" + message + "</color>" + " points of Physical damage<br>" + CurrentText;
        }
        else if (eventType == "Physical_CRITICAL")
        {
            AllText.text = "<color=#ce4627ff>"+ Name + "</color>" + " is <color=red>critically</color> hit taking  " + "<color=red>" + message + "</color>" + " points of damage<br>"+ CurrentText;
            CombatText.text = "<color=#ce4627ff>" + Name + "</color>" + " is <color=red>critically</color> hit taking  " + "<color=red>" + message + "</color>" + " points of damage<br>" + CurrentText;
        }
        else if (eventType == "HEAL")
        {
            message = "<color=green>" + message + "</color>";
            AllText.text = "<color=#ce4627ff>" + Name + "</color>" + " is healed for " + message + " points of damage<br>" + CurrentText;
            CombatText.text = "<color=#ce4627ff>" + Name + "</color>" + " is healed for " + message + " points of damage<br>" + CurrentText;
        }
        else if (eventType == "HEAL_CRITICAL")
        {
            message = "<color=green>" + message + "</color>";
            AllText.text = "<color=#ce4627ff>" + Name + "</color>" + " is critically healed for " + message + " points of damage<br>" + CurrentText;
            CombatText.text = "<color=#ce4627ff>" + Name + "</color>" + " is critically healed for " + message + " points of damage<br>" + CurrentText;
        }
    }
}
