using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{
    private AudioSource music;
    private AudioSource fadeIn;
    private AudioSource fadeOut;

    private AudioMixer mixer;

    private bool fadingInMusic = false;

    private float fadingSpeed = 0.005f;

    public AudioClip startingMusic;
    public AudioClip endingMusic;

    private int fadingMusicStage = 0;


    void Start()
    {
        mixer = Resources.Load("Master") as AudioMixer;

        music = gameObject.AddComponent<AudioSource>();
        music.outputAudioMixerGroup = mixer.FindMatchingGroups("Music")[0];
        music.loop = true;       
        
        music.clip = startingMusic;
        music.Play();
    }

    private void FixedUpdate()
    {
        if (fadingInMusic)
        {
            switch (fadingMusicStage)
            {
                case 0:
                    if (fadeOut.volume <= 0)
                    {
                        fadingMusicStage++;
                        break;
                    }
                    fadeOut.volume -= fadingSpeed;
                    break;

                case 1:
                    if (fadeIn.volume >= 1)
                    {
                        fadingMusicStage++;
                        break;
                    }
                    fadeIn.volume += fadingSpeed;
                    break;

                case 2:
                    music.clip = fadeIn.clip;
                    music.time = fadeIn.time;
                    music.Play();
                    fadeIn.Stop();

                    Destroy(fadeIn);
                    Destroy(fadeOut);

                    fadingMusicStage = 0;
                    fadingInMusic = false;
                    break;
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
