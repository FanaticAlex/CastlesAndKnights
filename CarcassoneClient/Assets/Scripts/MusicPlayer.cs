using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public AudioClip[] AudioClips;
    public AudioSource AudioSource;

    void Start()
    {
        AudioSource = FindObjectOfType<AudioSource>();
        AudioSource.loop = false;
    }

    void Update()
    {
        if (!AudioSource.isPlaying)
        {
            AudioSource.clip = GetRandomClip();
            AudioSource.Play();
        }
    }

    private AudioClip GetRandomClip()
    {
        return AudioClips[Random.Range(0, AudioClips.Length)];
    }
}
