using System.Collections.Generic;
using UnityEngine;

public class MusicScript : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] List<AudioClip> audioClips;
    [SerializeField] AudioClip victoryClip;
    [SerializeField] AudioClip defeatClip;

    private bool _isMusicOn;
    private bool _playOnlyOne = false;

    private void Start()
    {
        _isMusicOn = PlayerPrefs.GetInt("Music") == 1;
    }

    private void Update()
    {
        if (audioSource.isPlaying == false && _isMusicOn)
        {
            if (_playOnlyOne)
            {
                audioSource.Play();
            }
            else
            {
                PlayRandomClip();
            }
        }
    }

    private void PlayRandomClip()
    {
        int random = Random.Range(0, audioClips.Count);

        audioSource.clip = audioClips[random];
        audioSource.Play();
    }

    public void PlayVictoryMusic()
    {
        audioSource.clip = victoryClip;
        audioSource.Play();
        _playOnlyOne = true;
    }

    public void PlayDefeatMusic()
    {
        audioSource.clip = defeatClip;
        audioSource.Play();
        _playOnlyOne = true;
    }

    public void SetMusicOn(bool isMusicOn)
    {
        _isMusicOn = isMusicOn;

        if (isMusicOn == false)
        {
            audioSource.Stop();
        }
    }
}
