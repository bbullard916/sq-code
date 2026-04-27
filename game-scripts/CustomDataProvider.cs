using BLINK.RPGBuilder.Characters;
using System.Collections.Generic;
using UnityEngine;

public class CustomDataProvider : MonoBehaviour {

    private static CustomDataProvider instance = null;
    Dictionary<string, int> intData = new Dictionary<string, int>();
    Dictionary<string, string> stringData = new Dictionary<string, string>();
    Dictionary<string, bool> boolData = new Dictionary<string, bool>();
    Dictionary<string, Color> colorData = new Dictionary<string, Color>();
    public string currentClass;
    private static CustomDataProvider _instance;
    public string colorKey = "colorKey";
    public string stringKey = "stringKey";
    public string intKey = "intKey";
    public string boolKey = "boolKey";

    public static CustomDataProvider Instance
    {
        get { return _instance; }
    }

    private void Awake()
    {

        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        LoadData();                                     
    }

    public void LoadData()
    {
        if (ES3.KeyExists(colorKey))
        {
            colorData = ES3.Load(colorKey, colorData);
        }
        if (ES3.KeyExists(stringKey))
        {
            stringData = ES3.Load(stringKey, stringData);
        }

        if (ES3.KeyExists(intKey))
        {
            intData = ES3.Load(intKey, intData);
        }

        if (ES3.KeyExists(boolKey))
        {
            boolData = ES3.Load(boolKey, boolData);
        }
    }

    public void InitalizeData(string player)
    {
        string colorKey = "colorKey";
        if (!ES3.KeyExists(colorKey))
        {
            colorData["skin_tone"] = new Color(0,0,0);
            colorData["hair_color"] = new Color(0,0,0);

        }
        if (!ES3.KeyExists(stringKey))
        {
            stringData["test1"] = "test";
            stringData["Hair"] = "Hair 0";
            stringData["FacialHair"] = "FaceHair0";
            stringData["HairColor"] = "FFE100FF";
            stringData["SkinColor"] = "FFE396FF";
            //stringData["HairColor"] = "hair1";
            stringData["CurrentWorldMapTransition"] = "none";
            stringData["CurrentWorldMapPlayerLocation"] = "none";
            stringData["FactionBandits"] = "Bandits";
            stringData["FactionBanditsLeader"] = "Bandits";
            stringData["BanditsEnemy"] = "Bandits";
        }
         
        // Save Default string data firstTimeValues
        if (!ES3.KeyExists(intKey))
        {
            Debug.Log("RUNNING INITIAL SAVE");
            intData["test"] = 0;
            intData["PlayerWorldMapIcon"] = 0;
        }

        // Save Default string data firstTimeValues
        if (!ES3.KeyExists(boolKey))
        {
            boolData["worldmap foreground Stormwood Village"] = true;
            boolData["worldmap foreground Entwood Forest"] = true;
            boolData["worldmap foreground Bandit Hideout"] = true;
            boolData["worldmap foreground Stormwood Graveyard"] = false;
            boolData["Ship Wreck Fog"] = false;
            boolData["Frog Cave Fog"] = false;
            boolData["Storm Wood Village Fog"] = false;
            boolData["Stormwood Graveyard Fog"] = false;
            boolData["Entwood Forest Fog"] = false;
            boolData["Goblin Caverns Fog"] = false;
            boolData["Bandits Hideout Fog"] = false;
            boolData["Hidden Bandit Camp Fog"] = false;
            boolData["Ship Wreck Map Button"] = true;
            boolData["Stormwood Village Map Button"] = true;
            boolData["Entwood Forest Map Button"] = true; 
            boolData["Bandit Hideout Map Button"] = false;
        }
        ES3.Save(colorKey, colorData, ES3Settings.defaultSettings.path);
        ES3.Save(stringKey, stringData, ES3Settings.defaultSettings.path);
        ES3.Save(boolKey, boolData, ES3Settings.defaultSettings.path);
        ES3.Save(intKey, intData, ES3Settings.defaultSettings.path);
        // Load data if it exists
        if (ES3.KeyExists(colorKey))
        {
            colorData = ES3.Load(colorKey, colorData);
        }
        if (ES3.KeyExists(stringKey))
        {
            stringData = ES3.Load(stringKey, stringData);
        }

        if (ES3.KeyExists(intKey))
        {
            intData = ES3.Load(intKey, intData);
        }

        if (ES3.KeyExists(boolKey))
        {
            boolData = ES3.Load(boolKey, boolData);
        }
        PrintAllSaves();
    }

    public Color LoadColorData(string data)
    {
        return colorData[data];
    }
    public string LoadStringData(string data)
    {
        if (stringData.TryGetValue(data, out var value))
        {
            return value; // Return the value if the key exists
        }
        else
        {
            return null; // Return "null" if the key does not exist
        }
    }

    public int LoadIntData(string data)
    {
        return intData[data];
    }

    public bool LoadBoolData(string data)
    {
        if (boolData.TryGetValue(data, out bool value))
        {
            return value;
        }
        else
        {
            return false;
        }
    }


    public void SaveColorData(string key, Color value, string file)
    {
        string colorKey = "colorKey";
        colorData[key] = value;
        ES3.Save(colorKey, colorData, file);
    }
    public void SaveIntData(string key, int value, string file)
    {
        string intKey = "intKey";
        intData[key] = value;
        ES3.Save(intKey, intData, file);
    }

    public void SaveStringData(string key, string value, string file)
    {
        string stringKey = "stringKey";
        stringData[key] = value;
        ES3.Save(stringKey, stringData, file);
    }

    public void SaveBoolData(string key, bool value, string file)
    {
        string boolKey = "boolKey";
        boolData[key] = value;
        ES3.Save(boolKey, boolData, file);
    }

    public void PrintAllSaves()
    {
        //Debug.Log("String test: " + LoadStringData("test1"));
        //Debug.Log("int test: " + LoadIntData("test"));
    }

    public void ResetSaves(string player )
    {
        string fileName = player;
        if (ES3.FileExists("SaveFile.es3"))
        {
            ES3.RenameFile("SaveFile.es3", player);
        }
        ES3.DeleteFile(fileName);
        ES3Settings.defaultSettings.path = fileName;
        InitalizeData(player);
    }
}
