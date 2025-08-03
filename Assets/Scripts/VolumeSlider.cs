using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    private AudioSource audioManager;
    private float volume;
    public bool isSfx = false;
    private Slider slider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioManager = AudioManager.instance.bgmAudioSource;
        slider = GetComponent<Slider>();
        volume = (isSfx) ? AudioManager.instance.sfxVolume : audioManager.volume;
        slider.value = volume;
    }

    public void changeVolume()
    {
        if (!isSfx)
        {
            audioManager.volume = slider.value;
        }
        else
        {
            AudioManager.instance.sfxVolume = slider.value;
        }
    }
}
