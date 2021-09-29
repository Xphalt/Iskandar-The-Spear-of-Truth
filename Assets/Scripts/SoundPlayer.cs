using UnityEngine;

/*
 * Created by Mattie Hilton - 29/09/2021 
 */

[RequireComponent(typeof(AudioSource))]
public class SoundPlayer : MonoBehaviour
{
    public SoundLibrary.SoundGroup soundType;
    internal AudioSource aSource;

    private void Start()
    {
        aSource = GetComponent<AudioSource>();
    }            


    //When calling, make sure the correct soundType is set beforehand, then call PlayAudio
    public void PlayAudio()
    {
        Debug.Log(soundType + " " + SoundLibrary.instance.SoundLib[soundType].Count);
        aSource.clip = SoundLibrary.instance.SoundLib[soundType][Random.Range(0, SoundLibrary.instance.SoundLib[soundType].Count)];
        aSource.Play();
    }
}