using System.Collections.Generic;
using UnityEngine;

public class MusicScript : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] List<AudioClip> audioClips;

    private void Update()
    {
        if (audioSource.isPlaying == false)
        {
            PlayRandomClip();
        }
    }

    private void PlayRandomClip()
    {
        int random = Random.Range(0, audioClips.Count);

        audioSource.clip = audioClips[random];
        audioSource.Play();
    }
}
