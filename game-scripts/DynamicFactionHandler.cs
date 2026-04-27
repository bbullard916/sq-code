using UnityEngine;
using BLINK.RPGBuilder.AI;
using BLINK.RPGBuilder.Characters;
using BLINK.RPGBuilder.Combat;
using BLINK.RPGBuilder.Managers;
using MTAssets.EasyMinimapSystem;
using BLINK.RPGBuilder.UIElements;

public class DynamicFactionHandler : MonoBehaviour
{
    private AIEntity _AiEntity;
    public string FactionName;
    private RPGFaction otherFaction = null;
    public Sprite[] MiniMapIcons;
    public string FactionVal;

    public void Awake()
    {
        _AiEntity = this.gameObject.GetComponentInParent<AIEntity>();
        var faction = _AiEntity.ThisCombatEntity.GetFaction();
        LoadFaction();
        SetFaction("Bandit Warrior", "BanditsEnemy", "FactionBandits");
    }


    public void LateUpdate()
    {               
       LoadFaction();
    }
    private void LoadFaction()
    {                                                                                           
        string factionVal = CustomDataProvider.Instance.LoadStringData(FactionVal);
        otherFaction = GetFaction(factionVal);
        _AiEntity.ThisCombatEntity.SetFaction(otherFaction);
        if (factionVal.Contains("Enemy"))
        {
            _AiEntity.gameObject.tag = "enemy-npc";
            _AiEntity.gameObject.transform.GetChild(0).GetComponent<MinimapItem>().itemSprite = MiniMapIcons[2];
            _AiEntity.gameObject.layer = 0; 
        }
        else
        {
            _AiEntity.gameObject.tag = "npc";
            _AiEntity.gameObject.transform.GetChild(0).GetComponent<MinimapItem>().itemSprite = MiniMapIcons[1];
        }
        NameplatesDATAHolder[] go = GameObject.FindObjectsOfType<NameplatesDATAHolder>();
        foreach (var obj in go)
        {
            obj.SetColors("neutral");
            obj.UpdateTexts();
            obj.UpdateBar();
            obj.SetInteractionIcon();
            obj.SetScale();
        }
    }


    public void SetFaction(string combatEntity, string factionName, string loadSaveProvider)
    {
        Debug.Log("Setrting Faction: " + factionName);
        var entity = GetEntity(combatEntity);
        if (entity == null) return;
        var faction = GetFaction(factionName);
        if (faction == null) return;
        entity.SetFaction(faction);
        CustomDataProvider.Instance.SaveStringData(loadSaveProvider, factionName, Character.Instance.CharacterData.CharacterName);
        if (faction.name.Contains("Enemy"))
        {
            entity.gameObject.tag = "enemy-npc";
            entity.gameObject.transform.GetChild(0).GetComponent<MinimapItem>().itemSprite = MiniMapIcons[2];
            entity.gameObject.layer = 11;
        }
        else
        {
            entity.gameObject.tag = "npc";
            entity.gameObject.transform.GetChild(0).GetComponent<MinimapItem>().itemSprite = MiniMapIcons[1];

        }
        NameplatesDATAHolder[] go = GameObject.FindObjectsOfType<NameplatesDATAHolder>();
        foreach (var obj in go)
        {
            obj.SetColors("none");
            obj.UpdateTexts();
            obj.UpdateBar();
            obj.SetInteractionIcon();
            obj.SetScale();
        }
    }

    protected virtual CombatEntity GetEntity(string entityName)
    {
        foreach (CombatEntity entity in GameState.combatEntities)
        {
            if (entity == null) continue;
            if (string.IsNullOrEmpty(entityName) && entity is PlayerCombatEntity)
            {
                return entity;
            }
            else
            {
                var aiEntity = entity.GetComponent<AIEntity>();
                if (aiEntity != null && string.Equals(entity.name, entityName))
                {
                    return entity;
                }
            }
        }
        Debug.LogWarning($"Dialogue System: Can't find RPG Builder entity named '{entityName}'");
        return null;
    }
    protected virtual RPGFaction GetFaction(string factionName)
    {
        foreach (var faction in GameDatabase.Instance.GetFactions().Values)
        {
            if (faction != null && string.Equals(faction.entryName, factionName))
            {
                return faction;
            }
        }
        Debug.LogWarning($"Dialogue System: Can't find RPG Builder faction named '{factionName}'");
        return null;
    }
}
