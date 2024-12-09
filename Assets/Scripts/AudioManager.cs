using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public AudioMixer audioMixer; // Hubungkan Audio Mixer di Inspector
    public Slider volumeSlider;   // Slider untuk mengatur volume
    public Slider sfxSlider; //Slider untuk mengatur sfx
    public AudioClip[] sfxClips;
    public AudioSource[]sfxSources;
    private Dictionary<SfxCondition,AudioClip> sfxDictionary;
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        sfxDictionary = new Dictionary<SfxCondition,AudioClip>{
            {SfxCondition.birdsChirping, sfxClips[0]},
            {SfxCondition.boo, sfxClips[1]},
            {SfxCondition.boostAcc, sfxClips[2]},
            {SfxCondition.celebration, sfxClips[3]},
            {SfxCondition.crowdsCheering, sfxClips[4]},
            {SfxCondition.failed, sfxClips[5]},
            {SfxCondition.gallopDirt, sfxClips[6]},
            {SfxCondition.gallopMud, sfxClips[7]},
            {SfxCondition.gallopWater, sfxClips[8]},
            {SfxCondition.hit, sfxClips[9]},
            {SfxCondition.cleanRun, sfxClips[10]},
            {SfxCondition.mudBlast, sfxClips[11]},
            {SfxCondition.ram, sfxClips[12]},
            {SfxCondition.speedBoost, sfxClips[13]},
            {SfxCondition.power, sfxClips[14]},
            {SfxCondition.raceCountdowm, sfxClips[15]},
            {SfxCondition.raceStart, sfxClips[16]},
            {SfxCondition.victory, sfxClips[17]},
            {SfxCondition.waterStream, sfxClips[18]},
        };

        // Mengatur slider ke nilai saat ini jika sudah ada pengaturan default
        float currentVolume;
        audioMixer.GetFloat("MusicVolume", out currentVolume);
        volumeSlider.value = currentVolume;
        float currentSFXVolume;
        if (audioMixer.GetFloat("SfxVolume", out currentSFXVolume))
        {
            sfxSlider.value = currentSFXVolume;
        }
    }

    // Fungsi untuk mengubah volume
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", volume); // Ganti "MusicVolume" sesuai dengan exposed parameter
    }
    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SfxVolume", volume);
    }
     public void PlaySFX(SfxCondition condition)
    {
        if (sfxDictionary.TryGetValue(condition, out AudioClip clip))
        {
            // Cari audio source yang sedang tidak diputar
            foreach (var source in sfxSources)
            {
                if (!source.isPlaying)
                {
                    source.PlayOneShot(clip); // Memainkan SFX jika AudioSource tidak sedang memutar audio
                    return;
                }
            }
            
            // Jika semua AudioSource sedang diputar, pilih yang pertama dan ganti clip-nya
            sfxSources[0].PlayOneShot(clip); 
        }
        else
        {
            Debug.LogWarning("SFX untuk kondisi ini tidak ditemukan: " + condition);
        }
    }

}
