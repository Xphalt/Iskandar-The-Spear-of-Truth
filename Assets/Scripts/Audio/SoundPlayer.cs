using UnityEngine;
using System.Collections;
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
        mixer = Resources.Load("Master") as AudioMixer;
    }

    public void PlayAudio()
    {
        StartCoroutine(PlayAudioClip());
    }

    IEnumerator PlayAudioClip()
    {
        AddAudioSource();

        aSource.clip = audioClip;
        aSource.Play();

        yield return new WaitForSeconds(aSource.clip.length);
        Destroy(aSource);
    }

    private void AddAudioSource()
    {
        aSource = Camera.main.gameObject.AddComponent<AudioSource>();
        aSource.outputAudioMixerGroup = mixer.FindMatchingGroups("SFX")[0];
        aSource.loop = false;
        aSource.playOnAwake = false;
    }
}