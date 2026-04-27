using UnityEngine;
using System.Collections;
using BLINK.Controller;
using BLINK.RPGBuilder.Combat;
using BLINK.RPGBuilder.Characters;
using UnityEngine.SceneManagement;
using EPOOutline;
public class PlayerEventManager : MonoBehaviour
{
    public Animator PlayerAnimator;
    public RuntimeAnimatorController[] Controllers;
    public AudioSource PlayerAudioSource;
    public SoundManager _SoundManager;
    public TopDownClickToMoveController _TopDownClickToMoveController;
    public GameObject _StopRenderTexture;
    private SetPlayerCustomization _SetPlayerCustomization;
    private Outlinable outline;
    private void OnEnable()
    {
        CombatEvents.DamageDealt += DamageDealt;
        CombatEvents.CombatEntered += CombatStarted;
        CombatEvents.CombatExited += CombatEnded;
        GeneralEvents.PlayerGainedItem += GainedItem;
        GeneralEvents.PlayerEquippedItem += EquipItem;
        GeneralEvents.PlayerUnequippedItem += UnEquipItem;
        _SetPlayerCustomization = GetComponent<SetPlayerCustomization>();
        outline = GetComponent<Outlinable>();
    }

    private void OnDisable()
    {
        CombatEvents.DamageDealt -= DamageDealt;
        GeneralEvents.PlayerEquippedItem -= EquipItem;
        GeneralEvents.PlayerUnequippedItem -= UnEquipItem;
    }
    public void Start()
    {
        PlayerAnimator = GetComponent<Animator>();
        _SetPlayerCustomization = GetComponent<SetPlayerCustomization>();
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;
        if(currentScene.name.Contains("Main"))
        {
            PlayerAnimator.runtimeAnimatorController = Controllers[1];
            PlayerAnimator.SetBool("warriorIdle", true);
            PlayerAnimator.SetBool("wizardIdle", false);
        }
        else
        {
            PlayerAnimator.runtimeAnimatorController = Controllers[0];
        }
    }

    public IEnumerator handleHightlight()
    {
        yield return new WaitForSeconds(0.34f);
        outline.DrawingMode = OutlinableDrawingMode.Normal;
        yield return new WaitForSeconds(0.25f);
        outline.DrawingMode = OutlinableDrawingMode.ZOnly;
    }
    private void CombatStarted(CombatEntity entity)
    {
        if (entity.IsPlayer())
        {
            //SoundManager.Instance.PlayCombatMusic(PlayerAudioSource);
        }
    }

    private void GainedItem(RPGItem item, int count)
    {
        if(item.name.Contains("Bandit Hideout Map"))
        {
            CustomDataProvider.Instance.SaveBoolData("Bandit Hideout Map Button", true, Character.Instance.CharacterData.CharacterName);
        }
    }
    private void CombatEnded(CombatEntity entity)
    {
        if (entity.IsPlayer())
        {
            //SoundManager.Instance.StopCombatMusic(PlayerAudioSource);
        }
    }
    public void EquipItem(RPGItem itemEquipped)
    {
        StartCoroutine(ResetPortrait());
        if (itemEquipped.WeaponType && itemEquipped.weaponType == "Blunt Weapon" || itemEquipped.displayName == "Pick Axe")
        {
            SoundManager.Instance.PlayBluntWeaponEquip(PlayerAudioSource);
        }
        else if (itemEquipped.ArmorType && itemEquipped.ArmorType.entryName == "CLOTH")
        {
            SoundManager.Instance.PlayEquipCloth(PlayerAudioSource);
        } 
        else if(itemEquipped.ArmorSlot.entryDisplayName == "HELMET")
        {
            _SetPlayerCustomization.HelmetEquipped();
        }
    }

    public void UnEquipItem(RPGItem itemEquipped)
    {
        StartCoroutine(ResetPortrait());
        if (itemEquipped.WeaponType && itemEquipped.weaponType == "Blunt Weapon" || itemEquipped.displayName == "Pick Axe")
        {
            SoundManager.Instance.PlayBluntWeaponUnEquip(PlayerAudioSource);
        }
        else if (itemEquipped.ArmorType && itemEquipped.ArmorType.entryName == "CLOTH")
        {
            SoundManager.Instance.PlayUnEquipCloth(PlayerAudioSource);
        }
        else if (itemEquipped.ArmorSlot.entryDisplayName == "HELMET")
        {
            _SetPlayerCustomization.HelmetUnEquipped();
        }
    }

    private IEnumerator ResetPortrait()
    {
        _StopRenderTexture.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        //_StopRenderTexture.SetActive(false);
    }
    public void DamageDealt(CombatCalculations.DamageResult result)
    {
        Debug.Log(result.caster.name);
            if(!result.caster.IsPlayer())
        {
            SoundManager.Instance.PlayPlayerTakeDamage(PlayerAudioSource);
            PlayerAnimator.SetTrigger("GetHit");
            StartCoroutine(StunPlayer());
        }

    }
    public void PlayWalkSound()
    {
        SoundManager.Instance.PlayPlayerWalk(PlayerAudioSource);
    }

    public void PlayMiningSound()
    {
        SoundManager.Instance.PlayMining(PlayerAudioSource);
    }
    public void PlayDeathSound()
    {
        SoundManager.Instance.PlayPlayerDeath(PlayerAudioSource);
    }

    private IEnumerator StunPlayer()
    {
        _TopDownClickToMoveController.ResetAgentActions();
        _TopDownClickToMoveController.stunned = true;
        _TopDownClickToMoveController.movementEnabled = false;
        yield return new WaitForSeconds(0.35f);
        _TopDownClickToMoveController.stunned = false;
        _TopDownClickToMoveController.movementEnabled = true;
    }
}
