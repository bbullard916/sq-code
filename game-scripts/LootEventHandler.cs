using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootEventHandler : MonoBehaviour
{

    Rigidbody m_Rigidbody;
    public float m_Thrust = 200f;
    public void Start()
    {
        SoundManager.Instance.PlayLootDrop(GetComponent<AudioSource>());
        //Fetch the Rigidbody from the GameObject with this script attached
        //m_Rigidbody = GetComponent<Rigidbody>();
        //m_Rigidbody.AddForce(transform.up * m_Thrust);
    }
    void OnMouseDown()
    {
        Debug.Log(this.gameObject.name);
    }
}