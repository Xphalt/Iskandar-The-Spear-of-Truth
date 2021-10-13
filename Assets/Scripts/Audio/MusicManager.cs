using UnityEngine;
using UnityEditor;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{
    private AudioSource music;
    private AudioSource fadeIn;
    private AudioSource fadeOut;

    private AudioMixer mixer;

    private bool fadingInMusic = false;

    public float fadingSpeed = 0.05f;

    public AudioClip startingMusic;
    public AudioClip endingMusic;


    void Start()
    {
        mixer = Resources.Load("Master") as AudioMixer;

        music = gameObject.AddComponent<AudioSource>();
        music.outputAudioMixerGroup = mixer.FindMatchingGroups("Music")[0];
        music.loop = true;       
        
        music.clip = startingMusic;
        music.Play();
    }

    private void Update()
    {
        if (fadingInMusic)
        {
            fadeOut.volume -= fadingSpeed;
            fadeIn.volume += fadingSpeed;

            if (fadeOut.volume == 0f)
            {
                music.clip = fadeIn.clip;
                music.time = fadeIn.time;
                fadeIn.Stop();
                music.Play();
                fadeIn.volume = 0f;

                Destroy(fadeIn);
                Destroy(fadeOut);

                fadingInMusic = false;
            }
        }
    }

    public void FadeInMusic(AudioClip audioClip)
    {
        fadeIn = gameObject.AddComponent<AudioSource>();
        fadeIn.outputAudioMixerGroup = mixer.FindMatchingGroups("Fade In")[0];
        fadeIn.loop = true;

        fadeOut = gameObject.AddComponent<AudioSource>();
        fadeOut.outputAudioMixerGroup = mixer.FindMatchingGroups("Fade Out")[0];
        fadeOut.loop = true;

        fadeIn.volume = 0f;

        fadeOut.clip = music.clip;
        fadeOut.time = music.time;

        fadeOut.Play();
        music.Stop();

        fadeIn.clip = audioClip;
        fadeIn.Play();
        fadingInMusic = true;
    }
}
