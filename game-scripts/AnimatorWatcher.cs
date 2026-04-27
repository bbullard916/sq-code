using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorWatcher : MonoBehaviour
{
    public Animator CharAnimator;
    public string[] Attackanims = new string[] { "MeleeAttack1", "MeleeAttack2", "MeleeAttack3" };

    public void Update()
    {
        Debug.Log("Playing" + isPlaying(CharAnimator, Attackanims[0]));
    }
    public bool isPlaying(Animator anim, string stateName)
    {
        if (anim.GetCurrentAnimatorStateInfo(1).IsName(stateName) &&
                anim.GetCurrentAnimatorStateInfo(1).normalizedTime < 1.0f)
            return true;
        else
            return false;
    }
}
