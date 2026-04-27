using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;
using BLINK.RPGBuilder.AI;
using BLINK.RPGBuilder.Characters;
using BLINK.RPGBuilder.Combat;
using BLINK.RPGBuilder.Managers;

public class CustomLuaHandler : MonoBehaviour
{
    [Tooltip("Typically leave unticked so temporary Dialogue Managers don't unregister your functions.")]
    private void OnEnable()
    {
        Lua.RegisterFunction(nameof(DestroyTaggedObject), this, SymbolExtensions.GetMethodInfo(() => DestroyTaggedObject(string.Empty)));
        Lua.RegisterFunction(nameof(Fade), this, SymbolExtensions.GetMethodInfo(() => Fade((bool)true)));
        Lua.RegisterFunction(nameof(Fade), this, SymbolExtensions.GetMethodInfo(() => Fade((bool)true)));
        Lua.RegisterFunction(nameof(Pause), this, SymbolExtensions.GetMethodInfo(() => Pause((double)0)));
        Lua.RegisterFunction(nameof(GameObjectExists), this, SymbolExtensions.GetMethodInfo(() => GameObjectExists(string.Empty)));
        Lua.RegisterFunction(nameof(SendUIWindowText), this, SymbolExtensions.GetMethodInfo(() => SendUIWindowText(string.Empty)));
        Lua.RegisterFunction(nameof(SetCustomBoolData), this, SymbolExtensions.GetMethodInfo(() => SetCustomBoolData(string.Empty, (bool)true)));
        Lua.RegisterFunction(nameof(RpgbCustomChangeFaction), this, SymbolExtensions.GetMethodInfo(() => RpgbCustomChangeFaction(string.Empty, string.Empty, string.Empty)));
        Lua.RegisterFunction(nameof(SendCustomAlertMessage), this, SymbolExtensions.GetMethodInfo(() => SendCustomAlertMessage(string.Empty, (double)0)));
        Lua.RegisterFunction(nameof(SendSimpleAlertMessage), this, SymbolExtensions.GetMethodInfo(() => SendSimpleAlertMessage(string.Empty, string.Empty)));
    }

    void OnDisable()
    {
        Lua.UnregisterFunction(nameof(DestroyTaggedObject));
        Lua.UnregisterFunction(nameof(Fade));
        Lua.UnregisterFunction(nameof(Pause));
        Lua.UnregisterFunction(nameof(SendUIWindowText));
        Lua.UnregisterFunction(nameof(SendCustomAlertMessage));
        Lua.UnregisterFunction(nameof(SendSimpleAlertMessage));
         //Lua.UnregisterFunction(nameof(PlaySpendCoinSound));
        Lua.UnregisterFunction(nameof(SetCustomBoolData));
    }

    public bool GameObjectExists(string name)
    {
        return GameObject.Find(name) != null;
    }
    public void SendUIWindowText(string message)
    {
        UIInfoWindow.Instance.SendGeneralMessage(message);
    }

    public void SetCustomBoolData(string message, bool val)
    {
        CustomDataProvider.Instance.SaveBoolData(message, val, Character.Instance.CharacterData.CharacterName);
    }

    public void Pause(double val)
    {
        float myFloat = (float)val;
        StartCoroutine(PauseAction(myFloat));
    }
    public IEnumerator PauseAction(float val)
    {
        Debug.Log("Wait" + val);
        yield return new WaitForSeconds(val);
    }

    public void DestroyTaggedObject(string tag)
    {
        GameObject[] goList = GameObject.FindGameObjectsWithTag(tag);

        foreach (GameObject go in goList)
        {
            Destroy(go);
        }
    }


    public void RpgbCustomChangeFaction(string combatEntity, string factionName, string loadSaveProvider)
    {
        var entity = GetEntity(combatEntity);
        if (entity == null) return;
        entity.gameObject.transform.GetChild(0).GetComponent<DynamicFactionHandler>().SetFaction(combatEntity,factionName,loadSaveProvider);
        // var faction = GetFaction(factionName);
        Debug.Log("Setting Faction" + entity.name + "Faction:" + factionName);
        //if (faction == null) return;
        //entity.SetFaction(faction);
        //CustomDataProvider.Instance.SaveStringData(loadSaveProvider, factionName, Character.Instance.CharacterData.CharacterName);
    }

    public void SendCustomAlertMessage(string message, double IconID)
    {
        CustomAlertOverlay.Instance.SendCustomMessage(message, IconID);
    }

    public void SendSimpleAlertMessage(string message, string color)
    {
        SimpleCustomAlrtUI.Instance.SendCustomMessage(message, color);
    }

    public void Fade(bool val1)
    {
        if (val1 == true)
        {
            ScreenFader.Instance.FadeToBlack();
        }
        else if(val1 == false)
        {
            ScreenFader.Instance.FadeFromBlack();
        }
    }
    public void PlaySpendCoinSound()
    {
        SoundManager.Instance.PlayerCollectCoins(GameObject.FindGameObjectWithTag("PlayerAudioSource").GetComponent<AudioSource>());
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
    protected virtual CombatEntity GetEntity(string entityName)
    {
        foreach (CombatEntity entity in GameState.combatEntities)
        {
            Debug.Log("ENT" + entity.name);
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

}
