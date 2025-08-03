using UnityEngine;
using System.Security.Cryptography;
using UnityEngine.SceneManagement;
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [Tooltip("Make sure the load type for the bgm clips are set to streaming")]
    [SerializeField] private AudioClip[] BGM;
    [SerializeField] private AudioClip[] menuClickSFX;
    [SerializeField] private AudioClip[] menuStartSFX;
    [SerializeField] private AudioClip[] augmentPosSFX;
    [SerializeField] private AudioClip[] augmentNegSFX;
    public AudioSource bgmAudioSource;
    public AudioSource bgmSubAudioSource;
    public bool isUnderground;
    [SerializeField] private AudioSource menuSfxAudioSource;
    public float sfxVolume;
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sfxVolume = menuSfxAudioSource.volume;
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1 && bgmAudioSource.clip != BGM[1] && bgmAudioSource.clip != BGM[2])
        {
            playBGM(1,false);
        } else if (bgmAudioSource.clip == BGM[1] && !bgmAudioSource.isPlaying)
        {
            playBGM(2, true);
            bgmSubAudioSource.Play();
        } else if (SceneManager.GetActiveScene().buildIndex == 0 && bgmAudioSource.clip != BGM[0])
        {
            playBGM(1,true);
        }
        if (SceneManager.GetActiveScene().buildIndex == 1 && bgmAudioSource.clip == BGM[2] && isUnderground)
        {
            bgmAudioSource.mute = true;
            bgmSubAudioSource.mute = false;
        }
        else
        {
            bgmAudioSource.mute = false;
            bgmSubAudioSource.mute = true;
        }
        bgmSubAudioSource.volume = bgmAudioSource.volume;
        menuSfxAudioSource.volume = sfxVolume;
    }

    public void playBGM(int id, bool loop)
    {
        bgmAudioSource.clip = BGM[id];
        bgmAudioSource.loop = loop;
        bgmAudioSource.Play();
    }
    public void playMenuClick()
    {
        if (menuClickSFX.Length == 0) return;
        int rand = RandomNumberGenerator.GetInt32(0, menuClickSFX.Length);
        menuSfxAudioSource.clip = menuClickSFX[rand];
        menuSfxAudioSource.Play();
    }
    public void playMenuStart()
    {
        if (menuStartSFX.Length == 0) return;
        int rand = RandomNumberGenerator.GetInt32(0, menuStartSFX.Length);
        menuSfxAudioSource.clip = menuStartSFX[rand];
        menuSfxAudioSource.Play();
    }
    public void playAugmentPos()
    {
        if (augmentPosSFX.Length == 0) return;
        int rand = RandomNumberGenerator.GetInt32(0, augmentPosSFX.Length);
        menuSfxAudioSource.clip = augmentPosSFX[rand];
        menuSfxAudioSource.Play();
    }
    public void playAugmentNeg()
    {
        if (augmentNegSFX.Length == 0) return;
        int rand = RandomNumberGenerator.GetInt32(0, augmentNegSFX.Length);
        menuSfxAudioSource.clip = augmentNegSFX[rand];
        menuSfxAudioSource.Play();
    }

    public float updateVolume()
    {
        return sfxVolume;
    }
}
