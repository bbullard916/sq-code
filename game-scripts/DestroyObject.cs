using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers;

public class DestroyObject : MonoBehaviour
{
    public void Destroy()
    {
        DestroyImmediate(gameObject);
    }

    public void Disable()
    {
        this.gameObject.SetActive(false);
    }
}
