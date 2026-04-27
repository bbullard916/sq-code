using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
    // Start is called before the first frame update
    public void PlaySoundEffect(string sound)
    {
        if (sound == "bag")
        {
            SoundManager.Instance.OpenPlayerBag();
        }
        else if (sound == "DialogueAlert")
        {
            SoundManager.Instance.PlayDialogueAlertStinger();
        }
        else if(sound ==  "ClickSound1")
        {
            SoundManager.Instance.PlayUiClick(0);
        }
        else if (sound == "ClickSound2")
        {
            SoundManager.Instance.PlayUiClick(1);
        }
        else if (sound == "CoinSound")
        {
            AudioSource audio = GameObject.FindGameObjectWithTag("PlayerAudioSource").GetComponent<AudioSource>();
            SoundManager.Instance.PlayerCollectCoins(audio);
        }
    }
}
