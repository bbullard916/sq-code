using BLINK.RPGBuilder.Characters;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource PlayeerAudioSource;
    public AudioSource NPCAudioSource;
    public AudioClip[] PlayerWalk1;
    public AudioClip[] PlayerTakeDamage;
    public AudioClip[] PlayerDeath;
    public AudioClip[] BluntWeaponHit;
    public AudioClip[] AxeWeaponHit;
    public AudioClip[] SwordWeaponHit;
    public AudioClip[] CollectCoins;
    public AudioClip[] Mining;
    public AudioClip EquipItem;
    public AudioClip[] EquipMetalWeapon;
    public AudioClip[] EquipWoodenWeapon;
    public AudioClip[] UnEquipMetalWeapon;
    public AudioClip[] UnEquipWoodenWeapon;
    public AudioClip UnEquipItem;
    public AudioClip[] OpenCloseBag;
    public AudioClip[] FrogmanDeathSound;
    public AudioClip[] WolfDeathSound;
    public AudioClip[] OgreDeathSound;
    public AudioClip[] BanditDeathSound;
    public AudioClip[] GoblinDeathSound;
    public AudioClip[] KoboldWarriorDeathSound;
    public AudioClip[] FrogmanHitSound;
    public AudioClip[] WolfHitSound;
    public AudioClip[] OgreHitSound;
    public AudioClip[] BanditHitSound;
    public AudioClip[] TreeDwellerDeathSound;
    public AudioClip[] TreeDwellerHitSound;
    public AudioClip[] GoblinHitSound;
    public AudioClip[] KoboldWarriorHitSound;
    public AudioClip[] LootBagDropSound;
    public AudioClip[] InteractionSoundFx;
    public AudioClip[] EquipClothItem;
    public AudioClip[] UnEquipClothItem;
    public AudioClip[] Stingers;
    public AudioClip[] UIClick;
    public AudioClip[] AlchemistBenchInteract;
    public AudioClip[] OpenDoor;
    public AudioClip[] CloseDoor;

    private static SoundManager _instance;


    public static SoundManager Instance
    {
        get { return _instance; }
    }

    public void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
    #region Player Sound Effects
    public void PlayPlayerWalk(AudioSource audio)
    {
        audio.PlayOneShot(PlayerWalk1[Random.Range(0, PlayerWalk1.Length)]);
    }

    public void StopCombatMusic(AudioSource audio)
    {
        audio.Stop();
    }

    public void PlayPlayerTakeDamage(AudioSource audio)
    {
        audio.volume = 0.75f;
        audio.pitch = Random.Range(0.8f, 1.3f);
        audio.PlayOneShot(PlayerTakeDamage[Random.Range(0, PlayerTakeDamage.Length)]);
    }


    public void PlayMining(AudioSource audio)
    {
        audio.PlayOneShot(Mining[Random.Range(0, Mining.Length)]);
    }

    public void PlayBluntWeaponEquip(AudioSource audio)
    {
        audio.PlayOneShot(EquipWoodenWeapon[Random.Range(0, EquipWoodenWeapon.Length)]);
    }

    public void PlayOpenDoor(AudioSource audio)
    {
        audio.volume = 1f;
        audio.PlayOneShot(OpenDoor[Random.Range(0, OpenDoor.Length)]);
    }

    public void PlayCloseDoor(AudioSource audio)
    {
        audio.volume = 1f;
        audio.PlayOneShot(CloseDoor[Random.Range(0, CloseDoor.Length)]);
    }
    public void PlayEquipCloth(AudioSource audio)
    {
        audio.volume = 2f;
        audio.PlayOneShot(EquipClothItem[Random.Range(0, EquipWoodenWeapon.Length)]);
    }

    public void PlayUnEquipCloth(AudioSource audio)
    {
        audio.volume = 2f;
        audio.PlayOneShot(UnEquipClothItem[Random.Range(0, EquipWoodenWeapon.Length)]);
    }

    public void PlayBluntWeaponUnEquip(AudioSource audio)
    {
        audio.PlayOneShot(UnEquipWoodenWeapon[Random.Range(0, UnEquipWoodenWeapon.Length)]);
    }
    public void PlayPlayerDeath(AudioSource audio)
    {
        Debug.Log("Playing Death Sound");
        audio.PlayOneShot(PlayerDeath[Random.Range(0, PlayerDeath.Length)]);
    }

        public void PlayAlchemyWorkBenchInteract(AudioSource audio)
    {
        audio.PlayOneShot(AlchemistBenchInteract[Random.Range(0, AlchemistBenchInteract.Length)]);
    }

    public void PlayBluntWeaponHit(AudioSource audio)
    {
        audio.volume = 1.5f;
        audio.PlayOneShot(BluntWeaponHit[Random.Range(0, BluntWeaponHit.Length)]);
    }

    public void PlayAxeWeaponHit(AudioSource audio)
    {
        audio.PlayOneShot(AxeWeaponHit[Random.Range(0, AxeWeaponHit.Length)]);
    }

    public void PlayerCollectCoins(AudioSource audio)
    {
        audio.PlayOneShot(CollectCoins[Random.Range(0, CollectCoins.Length)]);
    }

    public void PlayDialogueAlertStinger()
    {
        AudioSource audio = GameObject.FindGameObjectWithTag("PlayerAudioSource").GetComponent<AudioSource>();
        audio.PlayOneShot(Stingers[0]);
    }

    public void PlayUiClick(int ID)
    {
        AudioSource audio = GameObject.FindGameObjectWithTag("PlayerAudioSource").GetComponent<AudioSource>();
        audio.volume = 0.4f;
        audio.PlayOneShot(UIClick[ID]);
    }
    public void PlayLevelUpStinger()
    {
        AudioSource audio = GameObject.FindGameObjectWithTag("PlayerAudioSource").GetComponent<AudioSource>();
        audio.PlayOneShot(Stingers[1]);
    }

    public void PlayAbilityStinger()
    {
        AudioSource audio = GameObject.FindGameObjectWithTag("PlayerAudioSource").GetComponent<AudioSource>();
        audio.PlayOneShot(Stingers[2]);
    }
    public void OpenPlayerBag()
    {
        AudioSource audio = GameObject.FindGameObjectWithTag("PlayerAudioSource").GetComponent<AudioSource>();
        audio.PlayOneShot(OpenCloseBag[Random.Range(0, OpenCloseBag.Length)]);
    }

    public void ClosePlayerBag()
    {
        AudioSource audio = GameObject.FindGameObjectWithTag("PlayerAudioSource").GetComponent<AudioSource>();
        audio.PlayOneShot(OpenCloseBag[Random.Range(0, OpenCloseBag.Length)]);
    }
    #endregion

    #region NPC Sound Effects

    public void PlayNpcDeathSound(string npc, AudioSource audio)
    {
        if (npc == "Frogman")
        {
            audio.volume = 2f;
            audio.PlayOneShot(FrogmanDeathSound[Random.Range(0, FrogmanDeathSound.Length)]);
        }
        if (npc == "Tree Dweller")
        {
            audio.volume = 1f;
            audio.PlayOneShot(TreeDwellerDeathSound[Random.Range(0, TreeDwellerDeathSound.Length)]);
        }
        if (npc == "Goblin")
        {
            audio.volume = 1f;
            audio.pitch = 1f;
            audio.PlayOneShot(GoblinDeathSound[Random.Range(0, GoblinDeathSound.Length)]);
        }
        if (npc == "Kobold Warrior")
        {
            audio.volume = 1f;
            audio.pitch = 1.6f;
            audio.PlayOneShot(KoboldWarriorDeathSound[Random.Range(0, KoboldWarriorDeathSound.Length)]);
        }
        if (npc == "Ogre")
        {
            audio.volume = 1f;
            audio.pitch = 1f;
            audio.PlayOneShot(OgreDeathSound[Random.Range(0, OgreDeathSound.Length)]);
        }
        if (npc == "Bandit Warrior")
        {
            audio.volume = 1f;
            audio.pitch = 1f;
            audio.PlayOneShot(BanditDeathSound[Random.Range(0, BanditDeathSound.Length)]);
        }
        if (npc == "Wolf")
        {
            audio.volume = 6f;
            audio.pitch = 1.4f;
            audio.PlayOneShot(WolfDeathSound[Random.Range(0, WolfDeathSound.Length)]);
        }
    }

    public void PlayNpcHitSound(string npc, AudioSource audio)
    {
        if (npc.Contains("Frogman"))
        {
            audio.volume = 1f;
            audio.pitch = Random.Range(1.4F, 1.6F);
            audio.PlayOneShot(FrogmanHitSound[Random.Range(0, FrogmanHitSound.Length)]);
        }
        if (npc.Contains("Tree Dweller") || npc.Contains("Entwood Minion"))
        {
            audio.volume = 0.5f;
            audio.pitch = 0.6f;
            audio.PlayOneShot(TreeDwellerHitSound[Random.Range(0, TreeDwellerHitSound.Length)]);
        }
        if (npc.Contains("Goblin"))
        {
            audio.volume = 1f;
            audio.pitch = Random.Range(0.7f, 1.6f);
            audio.PlayOneShot(GoblinHitSound[Random.Range(0, GoblinHitSound.Length)]);
        }
        if (npc.Contains("Kobold Warrior"))
        {
            audio.volume = 1f;
            audio.pitch = Random.Range(0.7f, 1.6f);
            audio.PlayOneShot(KoboldWarriorHitSound[Random.Range(0, KoboldWarriorHitSound.Length)]);
        }
        if (npc.Contains("Ogre"))
        {
            audio.volume = 1f;
            audio.pitch = 1f;
            audio.PlayOneShot(OgreHitSound[Random.Range(0, OgreHitSound.Length)]);
        }
        if (npc.Contains("Bandit"))
        {
            audio.volume = 0.5f;
            audio.pitch = 1.5f;
            audio.PlayOneShot(OgreHitSound[Random.Range(0, OgreHitSound.Length)]);
        }
        if (npc.Contains("Wolf"))
        {
            audio.volume = 0.6f;
            audio.pitch = 1.5f;
            audio.PlayOneShot(WolfHitSound[Random.Range(0, WolfHitSound.Length)]);
        }
    }

    public void PlayLootDrop(AudioSource audio)
    {
        audio.volume = 2;
        audio.PlayOneShot(LootBagDropSound[Random.Range(0, LootBagDropSound.Length)]);
    }

    public void PlayInteractioSoundFX(AudioSource audio, int id)
    {
        audio.PlayOneShot(InteractionSoundFx[id]);
    }
    #endregion
}
