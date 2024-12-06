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
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        // Mengatur slider ke nilai saat ini jika sudah ada pengaturan default
        float currentVolume;
        audioMixer.GetFloat("MasterVolume", out currentVolume);
        volumeSlider.value = currentVolume;
    }

    // Fungsi untuk mengubah volume
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("MasterVolume", volume); // Ganti "MusicVolume" sesuai dengan exposed parameter
    }

}
