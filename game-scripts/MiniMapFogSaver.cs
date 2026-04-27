using MTAssets.EasyMinimapSystem;                          
using UnityEngine;
using BLINK.RPGBuilder.Characters;
using System.Collections;
using UnityEngine.SceneManagement;

public class MiniMapFogSaver : MonoBehaviour
{
    public MinimapFog minimapFog;
    private static MiniMapFogSaver _instance;


    public static MiniMapFogSaver Instance
    {
        get { return _instance; }
    }

    public void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    public void Start()
    {
        string Scene = SceneManager.GetActiveScene().name;
        SetSaveID(Scene);
        if (CustomDataProvider.Instance.LoadBoolData(Scene+" Fog") == false)
        {
            minimapFog.DeleteFileOfSaveStateOfFogOfWar();
            
            CustomDataProvider.Instance.SaveBoolData(Scene + " Fog", true, Character.Instance.CharacterData.CharacterName);
        }
        else if (CustomDataProvider.Instance.LoadBoolData(Scene + " Fog") == true)
        {
            minimapFog.LoadStateOfFogOfWarAsync();
        }
        StartCoroutine(initializeTarget());
    }

    private void SetSaveID(string  ID)
    {
        minimapFog.fogsRevealedSaveId = minimapFog.fogsRevealedSaveId + "_" + ID + "_" + Character.Instance.CharacterData.CharacterName;
    }
    public void SaveFog()
    {
        minimapFog.SaveStateOfFogOfWar();
    }

    private IEnumerator initializeTarget()
    {
        yield return new WaitForSeconds(1f);
        minimapFog.targetsThatCanRemoveFog[0] = GameObject.FindGameObjectWithTag("Player").transform;
    }
}
