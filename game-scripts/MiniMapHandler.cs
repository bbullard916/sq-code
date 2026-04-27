using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using MTAssets.EasyMinimapSystem;
public class MiniMapHandler : MonoBehaviour
{
    public TextMeshProUGUI AreaText;
    public MinimapRenderer _MinimapRenderer;
    private void OnEnable()
    {
        GameEvents.NewGameSceneLoaded += SceneLoaded;
    }

    private void Start()
    {
        _MinimapRenderer = GetComponent<MinimapRenderer>();
    }

    private void OnDisable()
    {
        GameEvents.NewGameSceneLoaded -= SceneLoaded;
    }

    private void SceneLoaded()
    {
        Scene scene = SceneManager.GetActiveScene();
        AreaText.text = scene.name;
        ClearMiniMapHighlight();
    }

    private void  ClearMiniMapHighlight()
    {
        if (_MinimapRenderer.minimapItemsToHightlight.Count > 0)
        {
            for (int i = 0; i < _MinimapRenderer.minimapItemsToHightlight.Count; i++)
            {
                _MinimapRenderer.minimapItemsToHightlight.Remove(_MinimapRenderer.minimapItemsToHightlight[i]);
            }
        }
    }

    public void AddMiniMapHighlight(GameObject go )
    {
        _MinimapRenderer.AddMinimapItemToBeHighlighted(go.GetComponent<MinimapItem>());
    }
}
