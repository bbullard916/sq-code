using BLINK.RPGBuilder.Templates;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using BLINK.RPGBuilder.Characters;
public class WorldMapHandler : MonoBehaviour
{

    public GameActionsTemplate[] GameActionsTemplate;
    private CanvasGroup Cgroup;
    public Image[] PreviewImages;
    public TextMeshProUGUI TitleText;
    public TextMeshProUGUI DescriptionText;
    public GameObject[] ForegroundObject;
    public Transform[] PlayerPositions;
    public Transform PlayerIcon;
    public Button[] MapButtons;

    private void Start()
    {
        TitleText.text = "";
        DescriptionText.text = "";
        Cgroup = this.GetComponent<CanvasGroup>();
    }
    public void HandleTransition(string transition)
    {
        disableCG();
        if (transition == "stormwood village")
        {
            GameActionsManager.Instance.TriggerGameActions(GameState.playerEntity, GameActionsTemplate[0].GameActions);
        }
        else if (transition == "Entwood Forest")
        {
            GameActionsManager.Instance.TriggerGameActions(GameState.playerEntity, GameActionsTemplate[1].GameActions);
        }
        
    }

    private void  HandleMapButtons()
    {

    }
    public void SetMapForegroundObjectTrue(string name)
    {
        CustomDataProvider.Instance.SaveBoolData(name,true, Character.Instance.CharacterData.CharacterName);
    }

    public void SetMapForegroundObjectFalse(string name)
    {
        CustomDataProvider.Instance.SaveBoolData(name, false, Character.Instance.CharacterData.CharacterName);
    }

    public void OpenWorldMapPanel()
    {
        enableCG();
    }

    public void CloseWorldMapPanel()
    {
        disableCG();
    }
    public void disableCG()
    {
        Cgroup.interactable = false;
        Cgroup.blocksRaycasts = false;
        Cgroup.alpha = 0;
    }

    private void enableCG()
    {
        Cgroup.interactable = true;
        Cgroup.blocksRaycasts = true;
        for (int i = 0; i < ForegroundObject.Length; i++)
        {
            if (CustomDataProvider.Instance.LoadBoolData(ForegroundObject[i].name) == true)
            {
                ForegroundObject[i].SetActive(false);
            }
            else
            {
                ForegroundObject[i].SetActive(true);
            }
        }
        if (GameObject.FindGameObjectWithTag("player-worldmap"))
        {
            Destroy(GameObject.FindGameObjectWithTag("player-worldmap"));
        }
        Instantiate(PlayerIcon, PlayerPositions[CustomDataProvider.Instance.LoadIntData("PlayerWorldMapIcon")]);
        Cgroup.alpha = 1;
    }


    public void UpdatePlayerIconPosition(int pos)
    {
        if (GameObject.FindGameObjectWithTag("player-worldmap"))
        {
            Destroy(GameObject.FindGameObjectWithTag("player-worldmap"));
        }
        CustomDataProvider.Instance.SaveIntData("PlayerWorldMapIcon", pos, Character.Instance.CharacterData.CharacterName);
        Instantiate(PlayerIcon, PlayerPositions[CustomDataProvider.Instance.LoadIntData("PlayerWorldMapIcon")]);
    }

    public void OpenWorldMapRegion(string region)
    {
        if (region == "Stormwood Village")
        {
            CustomDataProvider.Instance.SaveBoolData("worldmap-foreground-area1", true, Character.Instance.CharacterData.CharacterName);
        }
    }
    public void HandlePointerEnter(string value)
    {
        Debug.Log("Pointer Enter: " + value);
        if (value.Contains("Storm Wood Village"))
        {
            PreviewImages[0].gameObject.SetActive(false);
            PreviewImages[2].gameObject.SetActive(false);
            PreviewImages[3].gameObject.SetActive(false);
            PreviewImages[1].gameObject.SetActive(true);
            TitleText.text = "Stormwood Village";
            DescriptionText.text = "This village is nestled into the coast, most recently the bandit attacks from the north have" +
                "disabled the village from it's trade routes";

        }
        if (value.Contains("Entwood Forest"))
        {
            PreviewImages[0].gameObject.SetActive(false);
            PreviewImages[1].gameObject.SetActive(false);
            PreviewImages[2].gameObject.SetActive(false);
            PreviewImages[3].gameObject.SetActive(true);
            TitleText.text = "Entwood Forest";
            DescriptionText.text = "The forest is over run by bandits and rumor has it " +
                "that the goblins have a cave hidden deep in the woods.";
        }
        if (value.Contains("The Meadowlands"))
        {
            PreviewImages[0].gameObject.SetActive(false);
            PreviewImages[1].gameObject.SetActive(false);
            PreviewImages[2].gameObject.SetActive(true);
            PreviewImages[3].gameObject.SetActive(false);
            TitleText.text = "The Meadowlands";
            DescriptionText.text = "Home to the HammerFall Inn, this regions connects the trade routes to the rest of the world.";
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
}
