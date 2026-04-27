using PixelCrushers;
using UnityEngine;

public class LevelWillBeChanged : MonoBehaviour
{

    public void levelWillChange()
    {
        SaveSystem.BeforeSceneChange();
    }
}
