using System.Collections.Generic;
using UnityEngine;

/*
 * Created by Mattie Hilton - 29/09/2021 
 */

public class SoundLibrary : MonoBehaviour
{
    public enum SoundGroup
    {
        PlayerCollect,
        PlayerMovement,
        PlayerDamage,
        CombatSwordWhoosh,
        CombatSwordHit,
        CombatStab
    }

    public List<AudioClip> PlayerCollect = new List<AudioClip>();
    public List<AudioClip> PlayerMovement = new List<AudioClip>();
    public List<AudioClip> PlayerDamage = new List<AudioClip>();
    public List<AudioClip> CombatSwordWhoosh = new List<AudioClip>();
    public List<AudioClip> CombatSwordHit = new List<AudioClip>();
    public List<AudioClip> CombatStab = new List<AudioClip>();


    internal Dictionary<SoundGroup, List<AudioClip>> SoundLib = new Dictionary<SoundGroup, List<AudioClip>>();

    public static SoundLibrary instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        SoundLib.Add(SoundGroup.PlayerCollect, PlayerCollect);
        SoundLib.Add(SoundGroup.PlayerMovement, PlayerMovement);
        SoundLib.Add(SoundGroup.PlayerDamage, PlayerDamage);
        SoundLib.Add(SoundGroup.CombatSwordWhoosh, CombatSwordWhoosh);
        SoundLib.Add(SoundGroup.CombatSwordHit, CombatSwordHit);
        SoundLib.Add(SoundGroup.CombatStab, CombatStab);
    }
}
