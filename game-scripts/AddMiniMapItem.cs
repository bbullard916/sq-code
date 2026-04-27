using MTAssets.EasyMinimapSystem;
using UnityEngine;
using System.Collections;
public class AddMiniMapItem : MonoBehaviour
{
    public MiniMapHandler _MiniMapHandler;

    private void Start()
    {
        StartCoroutine(Attach());
    }

    private void OnEnable()
    {
        StartCoroutine(Attach());
    }
    private IEnumerator Attach()
    {
        yield return new WaitForSeconds(0.4f);
        _MiniMapHandler = GameObject.Find("MiniMap").GetComponent<MiniMapHandler>();
        _MiniMapHandler.AddMiniMapHighlight(this.gameObject);
    }
}
