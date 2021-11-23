using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public bool inCombat = false;
    public bool checkForCombatUpdate = false;

    private AudioMixer mixer;

    internal AudioSource music;

    public MusicNode[] musicNodes;

    void Awake()
    {
        mixer = Resources.Load("Master") as AudioMixer;        

        foreach (MusicNode node in musicNodes)
        {
            node.mixer = mixer;
        }
    }

    private void FixedUpdate()
    {
        //if (checkForCombatUpdate)
        //{
        //    if (inCombat)
        //    {
        //        foreach (MusicNode node in musicNodes)
        //        {
        //            node.overridden = true;
        //        }

        //        music.clip = combatMusic;
        //        music.Play();
        //    }
        //    else
        //    {
        //        foreach (MusicNode node in musicNodes)
        //        {
        //            node.overridden = false;
        //        }

        //        music.clip = postCombatMusic;
        //        music.Play();
        //    }
        //    checkForCombatUpdate = false;
        //}
    }
}
