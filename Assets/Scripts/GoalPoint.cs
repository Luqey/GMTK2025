using UnityEngine;

public class GoalPoint : MonoBehaviour
{
    private Timer timer;
    [SerializeField] private GameObject endScreen;
    [SerializeField] private GameObject gameOverScreen;
    private AudioSource audioSource;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timer = FindFirstObjectByType<Timer>();
        audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && (endScreen == null || !endScreen.activeSelf) && (gameOverScreen == null || !gameOverScreen.activeSelf))
        {
            //End Level
            collision.gameObject.GetComponent<PlayerScript>().OnDisable();
            if (timer.isTimerActive()) timer.toggleTimer();
            timer.recordTimeScore();
            if (gameOverScreen != null && collision.GetComponent<LifeSystem>().getLives() <= 0)
            {
                gameOverScreen.SetActive(true);
            }
            else if (endScreen != null)
            {
                endScreen.SetActive(true);
                playSfx();
            }
        }
    }
    void playSfx()
    {
        if (audioSource.clip != null) audioSource.Play();
    }
}
