using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [Tooltip("Make sure the load type for the bgm clips are set to streaming")]
    [SerializeField] private AudioClip[] BGM;
    [SerializeField] private AudioClip[] menuSFX;
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
    public void playSFX(int id)
    {
        menuSfxAudioSource.clip = menuSFX[id];
        menuSfxAudioSource.Play();
    }
}
