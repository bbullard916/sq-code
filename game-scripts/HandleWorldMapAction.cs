using UnityEngine;
public class HandleWorldMapAction : MonoBehaviour
{
    public string transition;
    public CustomWorldMapHandler _WorldMapHandler;
    public int playerPosition;
    public void Start()
    {
        _WorldMapHandler = GameObject.FindGameObjectWithTag("World Map").GetComponent<CustomWorldMapHandler>();
    }
    public void HandleWorldMapActionCall(int playerPosition)
    {
        _WorldMapHandler.OpenWorldMapPanel();
        _WorldMapHandler.EnableMapButtons(transition, playerPosition);
    }

    public void ClosePanel()
    {
        _WorldMapHandler.CloseWorldMapPanel();
    }
}
