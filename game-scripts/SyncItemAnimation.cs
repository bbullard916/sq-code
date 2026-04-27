using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncItemAnimation : MonoBehaviour
{
    public Animator _CharAnimator;
    public Animator _ItemAnimator;
    // Start is called before the first frame update
    void Awake()
    {
        _CharAnimator = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        _ItemAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        for (int i = 0; i < _ItemAnimator.layerCount; i++)
        {
            _ItemAnimator.Play(_CharAnimator.GetCurrentAnimatorStateInfo(i).fullPathHash, i, _CharAnimator.GetCurrentAnimatorStateInfo(i).normalizedTime);
            _ItemAnimator.SetLayerWeight(i, _CharAnimator.GetLayerWeight(i));
        }
        //_ItemAnimator.Play(_CharAnimator.GetCurrentAnimatorStateInfo(0).fullPathHash, -1, _CharAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime);  
    }
}
