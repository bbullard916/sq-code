using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopRenderTexture : MonoBehaviour
{
    private Camera _CAM;
    public void Start()
    {
        _CAM = this.GetComponent<Camera>();
        //StartCoroutine(DisableCam());
    }

    public IEnumerator DisableCam()
    {
        yield return new WaitForSeconds(0.5f);
        //_CAM.enabled = false;
        //this.gameObject.SetActive(false);
    }

    public IEnumerator EnableCam()
    {
        yield return new WaitForSeconds(0.5f);
        this.gameObject.SetActive(true);
        _CAM.enabled = true;
    }

    public void ResetCam()
    {

    }
}
