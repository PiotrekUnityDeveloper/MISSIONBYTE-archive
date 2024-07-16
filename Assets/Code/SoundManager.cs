using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public List<Sound> sounds = new List<Sound>();
    public GameObject soundObject;
    public AudioSource sourceObj;

    private void Awake()
    {
        InitSounds();
    }

    private void Start()
    {
           
    }

    private void InitSounds()
    {
        foreach(AudioSource src in soundObject.GetComponents<AudioSource>())
        {
            Destroy(src);
        }

        foreach(Sound s in sounds)
        {
            s.soundSource = soundObject.AddComponent<AudioSource>();
            s.soundSource.loop = s.isLooping;
            s.soundSource.volume = s.soundVolume;
            s.soundSource.panStereo = s.soundPan;
            s.soundSource.playOnAwake = s.playOnAwake;
            s.soundSource.clip = s.soundClip;

            if(s.soundSource.playOnAwake == true)
            {
                s.soundSource.Play();
            }
        }
    }

    public void UpdateAndPlaySound(string soundName)
    {
        foreach (Sound s in sounds)
        {
            if (s.soundName == soundName)
            {
                s.soundSource.loop = s.isLooping;
                s.soundSource.volume = s.soundVolume;
                s.soundSource.panStereo = s.soundPan;
                s.soundSource.Play();
                break;
            }
        }
    }

    public void PlaySound(string soundName)
    {
        foreach(Sound s in sounds)
        {
            if(s.soundName == soundName)
            {
                s.soundSource.Play();
                break;
            }
        }
    }

    public void PlayInstantiatedSound(string soundName)
    {
        foreach (Sound s in sounds)
        {
            if (s.soundName == soundName)
            {
                //AudioSource asrc = Instantiate(new GameObject(), soundObject.transform.position, Quaternion.identity).AddComponent<AudioSource>();
                //asrc.gameObject.transform.parent = soundObject.transform;
                //AudioSource asrc = soundObject.AddComponent<AudioSource>();
                /*
                GameObject g = Instantiate(sourceObj.gameObject, this.transform.position, Quaternion.identity);
                g.transform.parent = soundObject.transform;
                AudioSource asrc = g.GetComponent<AudioSource>();
                asrc.clip = s.soundClip;
                asrc.panStereo = s.soundPan;
                asrc.volume = s.soundVolume;
                asrc.playOnAwake = true;
                asrc.Play();
                Destroy(asrc, asrc.clip.length * 2);
                break;*/ //PERFORMANCE UNFRIENDLY CODE DO NOT USE 
            }
        }
    }

    public void PauseSound(string soundName)
    {
        foreach (Sound s in sounds)
        {
            if (s.soundName == soundName)
            {
                s.soundSource.Pause();
                break;
            }
        }
    }
    public void UnPauseSound(string soundName)
    {
        foreach (Sound s in sounds)
        {
            if (s.soundName == soundName)
            {
                s.soundSource.UnPause();
                break;
            }
        }
    }

    public void StopSound(string soundName)
    {
        foreach (Sound s in sounds)
        {
            if (s.soundName == soundName)
            {
                s.soundSource.Stop();
                break;
            }
        }
    }

}
