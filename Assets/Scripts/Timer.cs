using System.Collections;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    private float time = 0f; //Time elapsed
    public float recordTime = 0f; //Fastest time that run
    public float previousTime = 0f; //Previous time score
    private bool isActive = false; //Boolean for whether the timer is active.
    private bool isRewinding = false;
    private LoopBuffMechanic loop;
    public LifeSystem lives;
    public PlayerScript player;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text countdownText;
    public TMP_Text recordTimeText;
    public TMP_Text previousTimeText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        loop = FindFirstObjectByType<LoopBuffMechanic>();
        lives = FindFirstObjectByType<LifeSystem>();
        player = FindFirstObjectByType<PlayerScript>();
        StartCoroutine("countdown");
        player.OnDisable();
        Debug.Log("OnDisable");
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            time += Time.deltaTime;
        }
        if (isRewinding)
        {
            time -= Time.deltaTime * 2.5f;
        }
        timerText.text = updateTime(time);
    }

    //Pauses or unpauses the timer
    public void toggleTimer()
    {
        isActive = !isActive;
    }
    public bool isTimerActive()
    {
        return isActive;
    }

    //Resets timer
    public void resetTimer()
    {
        time = 0f;
        timerText.text = updateTime(time);
        loop.Reset();
        player.recordGhost();
    }
    public void startRewind()
    {
        isRewinding = true;
    }
    //Records the time to the high score if it's faster, and adds it to the previous time
    public void recordTimeScore()
    {
        if (time < recordTime || recordTime == 0)
        {
            recordTime = time;
            recordTimeText.text = updateTime(recordTime);
        }
        else
        {
            lives.changeLives(-1);
        }
        previousTime = time;
        previousTimeText.text = updateTime(previousTime);
        loop.setDuration(previousTime);
    }

    //Returns a string for updating the timer text
    public string updateTime(float timeVar)
    {
        int miliseconds = ((int)(timeVar * 100)) % 100;
        string ms = (miliseconds < 10) ? "0" + miliseconds : miliseconds.ToString();
        int seconds = ((int)timeVar) % 60;
        string sec = (seconds < 10) ? "0" + seconds : seconds.ToString();
        int minutes = ((int)timeVar) / 60 % 360;
        string min = (minutes < 10) ? "0" + minutes : minutes.ToString();
        return min + ":" + sec + ":" + ms;
    }

    private IEnumerator countdown()
    {
        countdownText.text = "3";
        yield return new WaitForSeconds(0.5f);
        countdownText.text = "2";
        yield return new WaitForSeconds(0.5f);
        countdownText.text = "1";
        yield return new WaitForSeconds(0.5f);
        countdownText.text = "GO!";
        player.OnEnable();
        isActive = true;
        yield return new WaitForSeconds(0.5f);
        countdownText.gameObject.SetActive(false);
    }
}
