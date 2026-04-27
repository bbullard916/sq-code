using UnityEngine.EventSystems;
using UnityEngine;

public class InteractionSoundFX : MonoBehaviour
{
    public int SoundID;
    public AudioSource _AudioSource;

    public void PlayInteractionSound()
    {
        Debug.Log("Play Interaction Sound");
        //SoundManager.Instance.PlayInteractioSoundFX(_AudioSource, SoundID);
    }
}
