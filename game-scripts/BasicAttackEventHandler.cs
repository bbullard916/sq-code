using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttackEventHandler : MonoBehaviour
{
    public AudioSource PlayerAudioSource;
    public SoundManager _SoundManager;

    public void Awake()
    {
        PlayerAudioSource = GameObject.FindGameObjectWithTag("PlayerAudioSource").GetComponent<AudioSource>();
        _SoundManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<SoundManager>();
    }

    private void Start()
    {
            ReturnEquipType();
    }
    private void ReturnEquipType()
    {
        for (var index = 0; index < GameState.playerEntity.equippedWeapons.Count; index++)
        {
            if (GameState.playerEntity.equippedWeapons[index].item)
            {
                var equippedWeaponSlot = GameState.playerEntity.equippedWeapons[index];
                if (equippedWeaponSlot.item.WeaponType.name.Contains("Blunt"))
                {
                    //Debug.Log("Play Blunt Sound");
                    _SoundManager.PlayBluntWeaponHit(PlayerAudioSource);
                }
                if (equippedWeaponSlot.item.WeaponType.name.Contains("AXE"))
                {
                    Debug.Log("Play Axe Sound");
                    _SoundManager.PlayBluntWeaponHit(PlayerAudioSource);
                }
            }
        }
    }
}
