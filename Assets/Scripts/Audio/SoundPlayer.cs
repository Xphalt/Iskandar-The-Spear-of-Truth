using UnityEngine;
using UnityEngine.Audio;

/*
 * Created by Mattie Hilton - 29/09/2021 
 * Updated by Mattie Hilton - 26/10/2021 
 */

public class SoundPlayer : MonoBehaviour
{
    public AudioClip audioClip;
    internal AudioSource aSource;
    internal AudioMixer mixer;

    private void Start()
    {
        aSource = gameObject.AddComponent<AudioSource>();
        aSource.outputAudioMixerGroup = mixer.FindMatchingGroups("SFX")[0];
        aSource.loop = false;
        aSource.playOnAwake = false;
    }            

    //When calling, make sure the correct soundType is set beforehand, then call PlayAudio
    public void PlayAudio()
    {
        aSource.clip = audioClip;
        aSource.Play();
    }
}