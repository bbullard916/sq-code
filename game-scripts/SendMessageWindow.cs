using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendMessageWindow : MonoBehaviour
{
    // Start is called before the first frame update
    public UIInfoWindow _SoundManager;
    public string message;
    void Start()
    {
        _SoundManager = GameObject.FindGameObjectWithTag("info-text-window").GetComponent<UIInfoWindow>();
    }
    
    public void SendGeneralMessage(string text)
    {
        UIInfoWindow.Instance.SendGeneralMessage(message);
    }
}
