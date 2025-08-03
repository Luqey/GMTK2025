using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    private AudioSource audioManager;
    private float volume;
    private Slider slider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioManager = AudioManager.instance.bgmAudioSource;
        slider = GetComponent<Slider>();
        volume = audioManager.volume;
        slider.value = volume;
    }

    public void changeVolume()
    {
        audioManager.volume = slider.value;
    }
}
