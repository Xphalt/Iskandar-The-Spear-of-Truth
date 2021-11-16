using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public bool inCombat = false;
    public bool checkForCombatUpdate = false;

    public AudioClip startingMusic;
    public AudioClip combatMusic;
    public AudioClip postCombatMusic;

    private AudioMixer mixer;

    internal AudioSource music;

    public MusicNode[] musicNodes;

    void Start()
    {
        mixer = Resources.Load("Master") as AudioMixer;

        music = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<AudioSource>();
        music.outputAudioMixerGroup = mixer.FindMatchingGroups("Music")[0];
        music.loop = true;
        music.clip = startingMusic;
        music.Play();

        foreach (MusicNode node in musicNodes)
        {
            node.music = music;
            node.mixer = mixer;
        }
    }

    private void FixedUpdate()
    {
        if (checkForCombatUpdate)
        {
            if (inCombat)
            {
                foreach (MusicNode node in musicNodes)
                {
                    node.overridden = true;
                }

                music.clip = combatMusic;
                music.Play();
            }
            else
            {
                foreach (MusicNode node in musicNodes)
                {
                    node.overridden = false;
                }

                music.clip = postCombatMusic;
                music.Play();
            }
            checkForCombatUpdate = false;
        }
    }
}
