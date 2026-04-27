using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BLINK.RPGBuilder.Templates;
using BLINK.RPGBuilder.Characters;
using System.Collections.Generic;
using BLINK.RPGBuilder.Managers;
public class CustomWorldMapHandler : MonoBehaviour
{
    public GameActionsTemplate[] GameActionsTemplates;
    public Image[] PreviewImages;
    public TextMeshProUGUI TitleText;
    public TextMeshProUGUI DescriptionText;
    public GameObject[] ForegroundObjects;
    public Transform[] PlayerPositions;
    private string CurrentTransition;
    public Transform PlayerIcon;
    private Dictionary<string, List<string>> data = new Dictionary<string, List<string>>();
    private CanvasGroup canvasGroup;
    public Transform MapButtons;
    private string[] ForeGroundUI;

    private void Start()
    {
        TitleText.text = "";
        DescriptionText.text = "";
        canvasGroup = GetComponent<CanvasGroup>();
        SetValues("ShipWreckTransition_1", new List<string> { "Stormwood Village", "Bandit Hideout", "Entwood Forest" });
        SetValues("StormWoodVillageTransition_1", new List<string> { "Ship Wreck"});
        SetValues("StormWoodVillageTransition_2", new List<string> { "Entwood Forest", "Bandit Hideout"});
        SetValues("StormWoodVillageTransition_3", new List<string> { "Stormwood Graveyard" });
        SetValues("EntwoodForestTransition_1", new List<string> { "Stormwood Village"});
        var items = GetValues("StormWoodVillageTransition_2");
        ForeGroundUI = new string[] { "worldmap foreground Stormwood Village", "worldmap foreground Entwood Forest", "worldmap foreground Bandit Hideout",
        "worldmap foreground Stormwood Graveyard"};
    }

    private void HandleForegroundLayer()
    {
        for (int i = 0, count = ForeGroundUI.Length; i < count; i++)
        {
            bool status = CustomDataProvider.Instance.LoadBoolData(ForeGroundUI[i]);
            if (CustomDataProvider.Instance.LoadBoolData(ForeGroundUI[i]))
            {
                if (i ==  0)
                {
                    ForegroundObjects[0].SetActive(false);
                }
                if (i == 1)
                {
                    ForegroundObjects[1].SetActive(false);
                }
                if (i == 2)
                {
                    ForegroundObjects[2].SetActive(false);
                }
                if (i == 0)
                {
                    ForegroundObjects[3].SetActive(false);
                }
            }
            else
            {
                Debug.Log("MAP2:" + ForeGroundUI[i]);
                ForegroundObjects[i].SetActive(true);
            }
        }
    }

    public void EnableMapButtons(string transition,  int playerPos)
    {
        CurrentTransition = transition;
        var items = GetValues(transition);
        foreach (Transform child in MapButtons)
        {
            child.gameObject.SetActive(false);
        }
        for (int i = 0, count = items.Count; i < count; i++)
        {  
            foreach (Transform child in MapButtons)
            {
                if (child.gameObject.name ==  items[i])
                {
                    if(CustomDataProvider.Instance.LoadBoolData(items[i] + " Map Button"))
                      child.gameObject.SetActive(true);
                }
            }
        }
        HandleForegroundLayer();
        UpdatePlayerIconPosition(playerPos);
    }

    public void SetValues(string key, List<string> values)
    {
        if (data.ContainsKey(key))
        {
            data[key] = values; // Overwrite existing values
        }
        else
        {
            data.Add(key, values); // Add new key
        }
    }

    public void AddValue(string key, string value)
    {
        if (data.ContainsKey(key))
        {
            data[key].Add(value);
        }
        else
        {
            data[key] = new List<string> { value };
        }
    }

    // Get values for a key
    public List<string> GetValues(string key)
    {
        if (data.TryGetValue(key, out var values))
        {
            return values;
        }
        else
        {
            Debug.LogWarning($"Key '{key}' not found.");
            return new List<string>();
        }
    }

public void HandleTransition(string transition)
    {
        DisableCanvasGroup();
        CurrentTransition = transition;
        Debug.Log("tRANS" + CurrentTransition);
        switch (transition.ToLower())
        {
            case "stormwood village":
                CloseWorldMapPanel();
                if(CurrentTransition  == "ShipWreckTransition_1")
                {
                    GameActionsManager.Instance.TriggerGameActions(GameState.playerEntity, GameActionsTemplates[0].GameActions);
                }

                else if (CurrentTransition == "EntwoodForestTransition_1")
                {
                    GameActionsManager.Instance.TriggerGameActions(GameState.playerEntity, GameActionsTemplates[2].GameActions);
                }
                else if (CurrentTransition == "stormwood village")
                {
                    GameActionsManager.Instance.TriggerGameActions(GameState.playerEntity, GameActionsTemplates[0].GameActions);
                }
                break;

            case "entwood forest":
                CloseWorldMapPanel();
                if(CurrentTransition == "StormWoodVillageTransition_2")
                {
                    GameActionsManager.Instance.TriggerGameActions(GameState.playerEntity, GameActionsTemplates[3].GameActions);
                }
                else if (CurrentTransition == "entwood forest")
                {
                    GameActionsManager.Instance.TriggerGameActions(GameState.playerEntity, GameActionsTemplates[3].GameActions);
                }
                break;
            case "frog caverns":
                CloseWorldMapPanel();
                GameActionsManager.Instance.TriggerGameActions(GameState.playerEntity, GameActionsTemplates[4].GameActions);
                break;
            case "ship wreck":
                CloseWorldMapPanel();
                GameActionsManager.Instance.TriggerGameActions(GameState.playerEntity, GameActionsTemplates[5].GameActions);
                break;
            case "goblin caverns":
                CloseWorldMapPanel();
                GameActionsManager.Instance.TriggerGameActions(GameState.playerEntity, GameActionsTemplates[6].GameActions);
                break;
        }
    }

    public void SetMapForegroundObject(string name, bool state)
    {
        CustomDataProvider.Instance.SaveBoolData(name, state, Character.Instance.CharacterData.CharacterName);
    }

    public void OpenWorldMapPanel() => EnableCanvasGroup();

    public void CloseWorldMapPanel() => DisableCanvasGroup();

    private void DisableCanvasGroup()
    {
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0;
    }

    private void EnableCanvasGroup()
    {
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        var existingIcon = GameObject.FindGameObjectWithTag("player-worldmap");
        if (existingIcon != null) Destroy(existingIcon);

        int iconIndex = CustomDataProvider.Instance.LoadIntData("PlayerWorldMapIcon");
        Instantiate(PlayerIcon, PlayerPositions[iconIndex]);
        canvasGroup.alpha = 1;
    }

    public void UpdatePlayerIconPosition(int pos)
    {
        var existingIcon = GameObject.FindGameObjectWithTag("player-worldmap");
        if (existingIcon != null) Destroy(existingIcon);

        CustomDataProvider.Instance.SaveIntData("PlayerWorldMapIcon", pos, Character.Instance.CharacterData.CharacterName);
        Instantiate(PlayerIcon, PlayerPositions[pos]);
    }


    public void HandlePointerEnter(string value)
    {
        Debug.Log("Pointer Enter: " + value);

        HideAllPreviews();

        if (value.Contains("Storm Wood Village"))
        {
            SetPreview(1, "Stormwood Village",
                "This village is nestled into the coast. Recently, bandit attacks from the north have disrupted its trade routes.");
        }
        else if (value.Contains("Entwood Forest"))
        {
            SetPreview(3, "Entwood Forest",
                "The forest is overrun by bandits. Rumors say goblins have a hidden cave deep within.");
        }
        else if (value.Contains("The Meadowlands"))
        {
            SetPreview(2, "The Meadowlands",
                "Home to the HammerFall Inn, this region connects trade routes across the world.");
        }
    }

    public void HandlePointerExit(string value)
    {
        TitleText.text = "";
        DescriptionText.text = "";

        if (value.Contains("Storm Wood Village"))
        {
            PreviewImages[1].gameObject.SetActive(false);
            PreviewImages[0].gameObject.SetActive(true);
        }
        else
        {
            PreviewImages[1].gameObject.SetActive(true);
            PreviewImages[0].gameObject.SetActive(false);
        }
    }

    private void HideAllPreviews()
    {
        foreach (var img in PreviewImages)
        {
            img.gameObject.SetActive(false);
        }
    }

    private void SetPreview(int index, string title, string description)
    {
        if (index >= 0 && index < PreviewImages.Length)
        {
            PreviewImages[index].gameObject.SetActive(true);
        }
        TitleText.text = title;
        DescriptionText.text = description;
    }
}
