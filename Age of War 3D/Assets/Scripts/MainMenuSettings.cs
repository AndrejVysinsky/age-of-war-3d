using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuSettings : MonoBehaviour
{
    [SerializeField] GameObject musicToggleArrow;
    [SerializeField] GameObject soudEffectsToggleArrow;
    [SerializeField] MusicScript musicScript;

    private readonly string _music = "Music";
    private readonly string _soundEffects = "Sound Effects";

    private void Start()
    {
        if (PlayerPrefs.HasKey(_music) == false)
        {
            PlayerPrefs.SetInt(_music, 1);
        }

        if (PlayerPrefs.HasKey(_soundEffects) == false)
        {
            PlayerPrefs.SetInt(_soundEffects, 1);
        }
        PlayerPrefs.Save();

        bool isMusicOn = PlayerPrefs.GetInt(_music) == 1;
        bool areSoundEffectsOn = PlayerPrefs.GetInt(_soundEffects) == 1;

        musicToggleArrow.SetActive(isMusicOn);
        soudEffectsToggleArrow.SetActive(areSoundEffectsOn);
    }

    public void ToggleMusic()
    {
        bool isMusicOn = PlayerPrefs.GetInt(_music) == 1;

        isMusicOn = !isMusicOn;

        musicToggleArrow.SetActive(isMusicOn);

        musicScript.SetMusicOn(isMusicOn);

        PlayerPrefs.SetInt(_music, isMusicOn == true ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void ToggleSoundEffects()
    {
        bool areSoundEffectsOn = PlayerPrefs.GetInt(_soundEffects) == 1;

        areSoundEffectsOn = !areSoundEffectsOn;

        soudEffectsToggleArrow.SetActive(areSoundEffectsOn);

        PlayerPrefs.SetInt(_soundEffects, areSoundEffectsOn == true ? 1 : 0);
        PlayerPrefs.Save();
    }
}
