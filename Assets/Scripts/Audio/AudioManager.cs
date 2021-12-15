using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
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
}
