using UnityEngine;
using UnityEngine.Audio;

public class MusicNode : MonoBehaviour
{
    public AudioSource music;
    private AudioSource fadeIn;
    private AudioSource fadeOut;

    internal AudioMixer mixer;

    private bool fadingInMusic = false;

    public float fadingSpeed = 0.005f;
    public bool playOneTime = false;

    private int fadingMusicStage = 0;

    internal bool overridden = false;

    public AudioClip[] audioClips;
    public int amountToLoop = 0; //Going backwards to loop

    //E.g. LOOP3—>LOOP4—>LOOP5—>LOOP6—>(LOOP9—>LOOP4—>LOOP5—>LOOP6)…
    //The audio clip list is 3,4,5,6,9,4,5,6
    //Loop the last 4 by setting amountToLoop to 4

    public bool isLoopingManually = true;
    public bool isPlaying = false;
    internal float timeTillNextClip = 0f;
    internal int currentClip = 0;

    private void Start()
    {
        music = Camera.main.GetComponent<AudioSource>();
        music.outputAudioMixerGroup = mixer.FindMatchingGroups("Music")[0];
        //music.rolloffMode = AudioRolloffMode.Linear;
        //music.minDistance = 500f;
        music.priority = music.priority--;
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
                    if (playOneTime)
                        Destroy(gameObject);
                    break;
            }
        }


        else if (isPlaying)
        {
            if (isLoopingManually && music.clip.length <= timeTillNextClip)
            {
                timeTillNextClip = 0f;
                currentClip++;

                if (currentClip >= audioClips.Length)
                    currentClip = audioClips.Length-amountToLoop;

                music.PlayOneShot(audioClips[currentClip]);
                //Debug.Log("Playing: " + audioClips[currentClip]);
            }

            timeTillNextClip += Time.deltaTime;
        }
    }
    public void FadeInMusic()
    {
        Debug.Log("Fading in music");

        fadeIn = gameObject.AddComponent<AudioSource>();
        fadeIn.outputAudioMixerGroup = mixer.FindMatchingGroups("Fade In")[0];
        fadeIn.loop = true;

        fadeIn.clip = audioClips[0];

        fadeOut = gameObject.AddComponent<AudioSource>();
        fadeOut.outputAudioMixerGroup = mixer.FindMatchingGroups("Fade Out")[0];
        fadeOut.loop = true;

        fadeIn.volume = 0f;

        fadeOut.clip = music.clip;
        fadeOut.time = music.time;

        fadeOut.Play();
        music.Stop();

        music.clip = audioClips[0];
        isPlaying = true;
        timeTillNextClip = 0;
        currentClip = 0;
        fadingInMusic = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !overridden)
        {
            FadeInMusic();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && !overridden)
        {
            isPlaying = false;
        }
    }
}
