using UnityEngine;

public class GoalPoint : MonoBehaviour
{
    private Timer timer;
    [SerializeField] private GameObject endScreen;
    [SerializeField] private GameObject gameOverScreen;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timer = FindFirstObjectByType<Timer>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && (endScreen == null || !endScreen.activeSelf) && (gameOverScreen == null || !gameOverScreen.activeSelf))
        {
            //End Level
            Debug.Log("dumbtest-1");
            collision.gameObject.GetComponent<PlayerScript>().OnDisable();
            if (timer.isTimerActive()) timer.toggleTimer();
            timer.recordTimeScore();
            Debug.Log("dumbtest-0.5");
            if (gameOverScreen != null && collision.GetComponent<LifeSystem>().getLives() <= 0)
            {
                gameOverScreen.SetActive(true);
            }
            else if (endScreen != null)
            {
                endScreen.SetActive(true);
                Debug.Log("dumbtest-0.25");
            }
            Debug.Log("dumbtest-0.1");
        }
    }
}
