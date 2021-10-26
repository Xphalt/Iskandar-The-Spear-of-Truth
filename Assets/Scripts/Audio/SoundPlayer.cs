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





//using System.Collections.Generic;
//using UnityEngine;
//using UnityEditor;
///*
// * Created by Mattie Hilton - 29/09/2021 
// * Updated by Mattie Hilton - 26/10/2021
// */

//public class SoundLibrary : MonoBehaviour
//{
//    public enum SoundGroupParent
//    {
//        Player,
//        Combat,
//        Enemies
//    }
//    public enum SoundGroupChild
//    {
//        Breathing,
//        Footstep,
//        Damage,
//        Drinking,
//        Sword,
//        BowArrow,
//        Magic,
//        Armour
//    }
//    public enum SoundGroupChildChild
//    {
//        NULL,
//        Swing,
//        Slash,
//        Stab,
//        ArrowSet,
//        ArrowRelease,
//        ArrowCollide,
//        ArmourCollision
//    }

//    public Dictionary<SoundGroupParent, Dictionary<SoundGroupChild, Dictionary<SoundGroupChildChild, AudioClip[]>>> sfxClipLib =
//        new Dictionary<SoundGroupParent, Dictionary<SoundGroupChild, Dictionary<SoundGroupChildChild, AudioClip[]>>>();

//    public static SoundLibrary instance;

//    private void Awake()
//    {
//        instance = this;
//    }

//    private void Start()
//    {
//        SFXClip[] clips = Resources.FindObjectsOfTypeAll(typeof(SFXClip)) as SFXClip[];

//        foreach (var clip in clips)
//        {
//            Dictionary<SoundGroupChildChild, AudioClip[]> groupChildChild = new Dictionary<SoundGroupChildChild, AudioClip[]>
//            {
//                { clip.childChild, clip.audioClips }
//            };

//            Dictionary<SoundGroupChild, Dictionary<SoundGroupChildChild, AudioClip[]>> groupChild = new Dictionary<SoundGroupChild, Dictionary<SoundGroupChildChild, AudioClip[]>>
//            {
//                { clip.child, groupChildChild }
//            };

//            sfxClipLib.Add(clip.parent, groupChild);
//        }
//    }

//    public AudioClip[] GetAudioClips(SoundGroupParent parent, SoundGroupChild child, SoundGroupChildChild childChild = SoundGroupChildChild.NULL)
//    {
//        AudioClip[] audioClips;
//        Debug.Log(parent + " " + child + " " + childChild);
//        audioClips = sfxClipLib[parent][child][childChild];
//        return audioClips;
//    }
//}