using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    private float time = 0f; //Time elapsed
    private float recordTime = 0f; //Fastest time that run
    private float previousTime = 0f; //Previous time score
    private bool isActive = true; //Boolean for whether the timer is active.
    private LoopBuffMechanic loop;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text recordTimeText;
    [SerializeField] private TMP_Text previousTimeText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        loop = GameObject.FindGameObjectWithTag("Loop").GetComponent<LoopBuffMechanic>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            time += Time.deltaTime;
            timerText.text = updateTime(time);
        }
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
        recordTimeScore();
        time = 0f;
        timerText.text = updateTime(time);
        loop.Reset();
    }

    //Records the time to the high score if it's faster, and adds it to the previous time
    void recordTimeScore()
    {
        if (time < recordTime || recordTime == 0)
        {
            recordTime = time;
            recordTimeText.text = updateTime(recordTime);
        }
        previousTime = time;
        previousTimeText.text = updateTime(previousTime);
        loop.setDuration(previousTime);
    }

    //Returns a string for updating the timer text
    string updateTime(float timeVar)
    {
        int miliseconds = ((int)(timeVar * 100)) % 100;
        string ms = (miliseconds < 10) ? "0" + miliseconds : miliseconds.ToString();
        int seconds = ((int)timeVar) % 60;
        string sec = (seconds < 10) ? "0" + seconds : seconds.ToString();
        int minutes = ((int)timeVar) / 60 % 360;
        string min = (minutes < 10) ? "0" + minutes : minutes.ToString();
        return min + ":" + sec + ":" + ms;
    }
}
