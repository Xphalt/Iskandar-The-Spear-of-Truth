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
    public enum FootstepType {stone, defaultMass, wood, metal};

    public List<AudioClip> footstepsStone = new List<AudioClip>();
    public List<AudioClip> footstepsDefault = new List<AudioClip>();
    public List<AudioClip> footstepsWood = new List<AudioClip>();
    public List<AudioClip> footstepsMetal = new List<AudioClip>();
    public List<AudioClip> armourHit = new List<AudioClip>();
    public List<AudioClip> swordSwing = new List<AudioClip>();

    internal Dictionary<AudioType, List<AudioClip>> playerSFXDictionary = new Dictionary<AudioType, List<AudioClip>>();
    internal Dictionary<FootstepType, List<AudioClip>> playerFootstepDictionary = new Dictionary<FootstepType, List<AudioClip>>();

    public AudioType audioType;
    public FootstepType footstepType;


    private void Start()
    {
        mixer = Resources.Load("Master") as AudioMixer;

        playerSFXDictionary.Add(AudioType.armourHit, armourHit);
        playerSFXDictionary.Add(AudioType.swordSwing, swordSwing);

        playerFootstepDictionary.Add(FootstepType.stone, footstepsStone);
        playerFootstepDictionary.Add(FootstepType.defaultMass, footstepsDefault);
        playerFootstepDictionary.Add(FootstepType.wood, footstepsWood);
        playerFootstepDictionary.Add(FootstepType.metal, footstepsMetal);

        aSource = new AudioSource[playerSFXDictionary.Count+1];

        for (int i = 0; i < playerSFXDictionary.Count+1; i++)
        {
            aSource[i] = gameObject.AddComponent<AudioSource>();
            aSource[i].outputAudioMixerGroup = mixer.FindMatchingGroups("SFX")[0];
            aSource[i].loop = false;
            aSource[i].playOnAwake = false;
        }
    }

    public void SetAudioType(AudioType at)
    {
        audioType = at;
    }

    public void PlayAudio()
    {      
        if(audioType == AudioType.footsteps)
            aSource[aSourceCounter].clip = playerFootstepDictionary[footstepType][Random.Range(0, playerFootstepDictionary[footstepType].Count)];

        else aSource[aSourceCounter].clip = playerSFXDictionary[audioType][Random.Range(0, playerSFXDictionary[audioType].Count)];

        aSource[aSourceCounter].Play();
        aSourceCounter++;

        if (aSourceCounter >= aSource.Length)
            aSourceCounter = 0;
    }
}
