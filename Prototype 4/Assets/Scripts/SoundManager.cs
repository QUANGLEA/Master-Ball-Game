using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    static readonly string FirstPlay = "FirstPlay";
    static readonly string BackgroundPref = "BackgroundPref";
    static readonly string SoundEffectsPref = "SoundEffectsPref";
    int firstPlayInt;

    [SerializeField] Slider[] backgroundSliders;
    [SerializeField] Slider[] soundEffectsSliders;
    float backgroundFloat, soundEffectsFloat;

    [SerializeField] AudioSource backgroundAudio;
    [SerializeField] AudioSource[] soundEffectsAudio;

    // Start is called before the first frame update
    void Start()
    {
        firstPlayInt = PlayerPrefs.GetInt(FirstPlay);

        if (firstPlayInt == 0)
        {
            backgroundFloat = .75f;
            soundEffectsFloat = .8f;

            for (int i = 0; i < backgroundSliders.Length; i++)
            {
                backgroundSliders[i].value = backgroundFloat;
                soundEffectsSliders[i].value = soundEffectsFloat;
            }

            PlayerPrefs.SetFloat(BackgroundPref, backgroundFloat);
            PlayerPrefs.SetFloat(SoundEffectsPref, soundEffectsFloat);
            PlayerPrefs.SetInt(FirstPlay, -1);
        }
        else
        {
            backgroundFloat = PlayerPrefs.GetFloat(BackgroundPref);
            soundEffectsFloat = PlayerPrefs.GetFloat(SoundEffectsPref);
            for (int i = 0; i < backgroundSliders.Length; i++)
            {
                backgroundSliders[i].value = backgroundFloat;
                soundEffectsSliders[i].value = soundEffectsFloat;
            }
        }
    }

    public void SaveSoundSettings()
    {
        for (int i = 0; i < backgroundSliders.Length; i++)
        {
            PlayerPrefs.SetFloat(BackgroundPref, backgroundSliders[i].value);
            PlayerPrefs.SetFloat(SoundEffectsPref, soundEffectsSliders[i].value);
        }
            
    }

    private void OnApplicationFocus(bool focus)
    {
        if (!focus)
        {
            SaveSoundSettings();
        }
    }

    public void UpdateSound()
    {
        backgroundAudio.volume = backgroundSliders[0].value;


        for (int i = 0; i < soundEffectsAudio.Length; i++)
        {
            
            soundEffectsAudio[i].volume = soundEffectsSliders[0].value;
            
        }
    }

    public void UpdateSound2()
    {
        backgroundAudio.volume = backgroundSliders[1].value;


        for (int i = 0; i < soundEffectsAudio.Length; i++)
        {

            soundEffectsAudio[i].volume = soundEffectsSliders[1].value;

        }
    }

}
