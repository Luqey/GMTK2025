using UnityEngine;
using UnityEngine.SceneManagement;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;
    private GameObject player;
    private Timer timer;
    private Vector2 initialPlayerPosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    void Start()
    {
        player = FindFirstObjectByType<PlayerScript>().gameObject;
        initialPlayerPosition = player.transform.position;
        timer = FindFirstObjectByType<Timer>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void resetLevel(GameObject endScreen)
    {
        player.transform.position = initialPlayerPosition;
        player.GetComponent<PlayerScript>().OnEnable();
        timer.resetTimer();
        if (!timer.isTimerActive()) timer.toggleTimer();
        endScreen.SetActive(false);
    }
}
