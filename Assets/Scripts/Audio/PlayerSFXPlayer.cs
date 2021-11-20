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
    public enum FootstepType {stone, dirt};

    public List<AudioClip> footstepsStone = new List<AudioClip>();
    public List<AudioClip> footstepsDirt = new List<AudioClip>();
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
        playerFootstepDictionary.Add(FootstepType.dirt, footstepsDirt);

        aSource = new AudioSource[playerSFXDictionary.Count+1];

        for (int i = 0; i < playerSFXDictionary.Count+1; i++)
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

        if(audioType == AudioType.footsteps)
            aSource[aSourceCounter].clip = playerFootstepDictionary[footstepType][Random.Range(0, playerFootstepDictionary[footstepType].Count)];

        else aSource[aSourceCounter].clip = playerSFXDictionary[audioType][Random.Range(0, playerSFXDictionary[audioType].Count)];

        aSource[aSourceCounter].Play();
        aSourceCounter++;

        if (aSourceCounter >= aSource.Length)
            aSourceCounter = 0;
    }
}
