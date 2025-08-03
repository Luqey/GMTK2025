using UnityEngine;
using System.Security.Cryptography;
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
    [SerializeField] private AudioSource menuSfxAudioSource;
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

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void playBGM(int id)
    {
        bgmAudioSource.clip = BGM[id];
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
}
