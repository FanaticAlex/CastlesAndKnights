using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public AudioClip[] AudioClips;
    public AudioSource AudioSource;
    private int _currentClipNumber;

    void Start()
    {
        AudioSource = FindObjectOfType<AudioSource>();
        AudioSource.loop = false;
        PlayClip(_currentClipNumber);
        _currentClipNumber = 0;
    }

    void Update()
    {
        if (!AudioSource.isPlaying)
        {
            _currentClipNumber = (_currentClipNumber + 1) % AudioClips.Length;
            PlayClip(_currentClipNumber);
        }
    }

    private void PlayClip(int clipNumber)
    {
        AudioSource.clip = AudioClips[clipNumber];
        AudioSource.Play();
    }
}
