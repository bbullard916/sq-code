using UnityEngine;

public class HighlightOnHover : MonoBehaviour
{
    private Renderer rend;
    public Material originalMat;
    public Material highlightMat;

    void Start()
    {
        rend = GetComponent<Renderer>();
        if (rend != null)
            originalMat = rend.material;
    }

    void OnMouseEnter()
    {
        if (rend != null && highlightMat != null)
            rend.material = highlightMat;
    }

    void OnMouseExit()
    {
        if (rend != null && originalMat != null)
            rend.material = originalMat;
    }
}
