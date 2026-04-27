using UnityEngine;
using BLINK.RPGBuilder.Combat;
using System.Collections;
using EPOOutline;
public class NpcEventManager : MonoBehaviour
{
    public Animator NpcAnimator;
    public AudioSource NPCAudioSource;
    public string npc;
    private Outlinable outline;
    public float rotationSpeed = 2.0f;  // Rotation speed in degrees per second

    public IEnumerator handleHightlight()
    {
        yield return new WaitForSeconds(0.34f);
        outline.DrawingMode = OutlinableDrawingMode.Normal;
        yield return new WaitForSeconds(0.25f);
        outline.DrawingMode = OutlinableDrawingMode.ZOnly;
    }
    private void OnEnable()
    {
        CombatEvents.EntityDied += Death;
        CombatEvents.DamageDealt += NpcHit;
        outline = GetComponent<Outlinable>();
    }

    private void OnDisable()
    {
        CombatEvents.EntityDied -= Death;
        CombatEvents.DamageDealt -= NpcHit;
    }
    public void Start()
    {
        NpcAnimator = gameObject.GetComponentInParent<Animator>();
    }

    public void PlayNpcdeathSound()
    {
        SoundManager.Instance.PlayNpcDeathSound(npc, NPCAudioSource);
    }


    public void PlayNpcHit()
    {
        SoundManager.Instance.PlayNpcHitSound(npc, NPCAudioSource);
    }
    private void Death(CombatEntity entity)
    {
        if(entity != null && GetComponentInParent<CombatEntity>().IsDead())
        {
            PlayNpcdeathSound();
            NpcAnimator.SetTrigger("Death");
        }
    }

    private void NpcHit(CombatCalculations.DamageResult result)
    {
        if (!result.target.IsDead() && !result.target.IsPlayer()  && result.target.IsNPC())
        {
            PlayNpcHit();
        }
    }
}
