using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(Instance);
        }
        Instance = this;
    }

    public AudioSource _audioPlayer;
    public AudioClip gridSelectSFX;
    public AudioClip gridPutSFX;
    public AudioClip gridShuffleSFX;

    public void PlayAudio(AudioClip audio)
    {
        _audioPlayer.PlayOneShot(audio);
    }

    public void Play_GridSelectSFX()
    {
        _audioPlayer.PlayOneShot(gridSelectSFX);
    }

    public void Play_GridShuffleSFX()
    {
        _audioPlayer.PlayOneShot(gridShuffleSFX);
    }

    public void Play_GridPutSFX()
    {
        _audioPlayer.PlayOneShot(gridPutSFX);
    }
}
