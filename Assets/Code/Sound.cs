using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string soundName;
    public float soundVolume = 0.8f;
    [Range(-1, 1)] public float soundPan = 0.0f;

    public bool isLooping = false;
    public bool playOnAwake = false;

    public AudioSource soundSource;
    public AudioClip soundClip;
}
