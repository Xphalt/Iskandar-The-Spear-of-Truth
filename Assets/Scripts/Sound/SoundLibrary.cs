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
        PlayerDamage
    }

    public List<AudioClip> PlayerCollect = new List<AudioClip>();
    public List<AudioClip> PlayerMovement = new List<AudioClip>();
    public List<AudioClip> PlayerDamage = new List<AudioClip>();

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
    }
}
