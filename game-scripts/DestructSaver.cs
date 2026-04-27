using BLINK.RPGBuilder.Characters;
using UnityEngine;

public class DestructSaver : MonoBehaviour
{
    public string SceneKey;
    void Start()
    {
        if (CustomDataProvider.Instance.LoadBoolData(gameObject.name + "_destruct_" + SceneKey + Character.Instance.CharacterData.CharacterName) == true)
        {
            DestroyImmediate(this.gameObject);
        }
    }
    // Update is called once per frame
    public void DestroyObject()
    {
            CustomDataProvider.Instance.SaveBoolData(gameObject.name + "_destruct_"+SceneKey+ Character.Instance.CharacterData.CharacterName, true, Character.Instance.CharacterData.CharacterName);
    }
}
