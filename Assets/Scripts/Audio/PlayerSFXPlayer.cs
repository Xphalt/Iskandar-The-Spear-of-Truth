using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PlayerSFXPlayer : MonoBehaviour
{
    internal AudioMixer mixer;

    internal AudioSource[] aSource;
    int aSourceCounter = 0;

    public enum AudioType {footsteps, armourHit, swordSwing };

    public List<AudioClip> footsteps = new List<AudioClip>();
    public List<AudioClip> armourHit = new List<AudioClip>();
    public List<AudioClip> swordSwing = new List<AudioClip>();

    internal Dictionary<AudioType, List<AudioClip>> playerSFXDictionary = new Dictionary<AudioType, List<AudioClip>>();

    public AudioType audioType;


    private void Start()
    {
        mixer = Resources.Load("Master") as AudioMixer;

        playerSFXDictionary.Add(AudioType.footsteps, footsteps);
        playerSFXDictionary.Add(AudioType.armourHit, armourHit);
        playerSFXDictionary.Add(AudioType.swordSwing, swordSwing);

        aSource = new AudioSource[playerSFXDictionary.Count];

        for (int i = 0; i < playerSFXDictionary.Count; i++)
        {
            aSource[i] = gameObject.AddComponent<AudioSource>();
        }
    }

    public void SetAudioType(AudioType at)
    {
        audioType = at;
    }

    public void PlayAudio()
    {
        aSource[aSourceCounter].outputAudioMixerGroup = mixer.FindMatchingGroups("SFX")[0];
        aSource[aSourceCounter].loop = false;
        aSource[aSourceCounter].playOnAwake = false;

        aSource[aSourceCounter].clip = playerSFXDictionary[audioType][Random.Range(0, playerSFXDictionary[audioType].Count)];
        aSource[aSourceCounter].Play();
        aSourceCounter++;
        if (aSourceCounter >= aSource.Length)
            aSourceCounter = 0;
    }
}
