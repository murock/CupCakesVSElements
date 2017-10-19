using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : Singleton<SoundManager> {

    [SerializeField]
    private AudioSource musicSource;

    [SerializeField]
    private AudioSource sfxSource;


    [SerializeField]
    private Slider musicSlider;

    [SerializeField]
    private Slider sfxSlider;

    Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();

    // Use this for initialization
    void Start () {
        AudioClip[] clips = Resources.LoadAll<AudioClip>("Audio") as AudioClip[];

        foreach (AudioClip clip in clips)
        {
            audioClips.Add(clip.name, clip);    //each audio clip stored by its name
        }

        LoadVolume();   //load saved volumes

        musicSlider.onValueChanged.AddListener(delegate { UpdateVolume(); });   //add updatevoulme to on value changed event
        sfxSlider.onValueChanged.AddListener(delegate { UpdateVolume(); });
    }
	
    public void PlaySFX(string name)
    {
        sfxSource.PlayOneShot(audioClips[name]);
    }

    public void UpdateVolume()
    {
        musicSource.volume = musicSlider.value;

        sfxSource.volume = sfxSlider.value;

        PlayerPrefs.SetFloat("SFX", sfxSlider.value);   //save for next play
        PlayerPrefs.SetFloat("Music", musicSlider.value);
    }

    public void LoadVolume()
    {
        sfxSource.volume = PlayerPrefs.GetFloat("SFX", 0.5f);   //second value is default for when game is first started
        musicSource.volume = PlayerPrefs.GetFloat("Music", 0.5f);

        musicSlider.value = musicSource.volume;
        sfxSlider.value = sfxSource.volume;
    }
}
