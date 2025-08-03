using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;
    private GameObject player;
    private Timer timer;
    [SerializeField] private RectTransform pauseScreen;
    [SerializeField] private GameObject endScreen;
    [SerializeField] private GameObject countdownTimer;
    public TMP_Text rewindText;
    [SerializeField] private Sprite rewindSpr;
    public Vector2 pauseScreenAnchorPos;
    public Vector2 initialPlayerPosition;
    [SerializeField] RectTransform gameOverScreen;
    public LoopBuffMechanic lbm;
    public bool failed = false;
    public bool gameOver;
    public bool isPaused = false;
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
        pauseScreenAnchorPos = pauseScreen.position;
        pauseScreen.position += new Vector3(0, 1000f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (!endScreen.activeSelf && !rewindText.gameObject.activeSelf && !countdownTimer.activeSelf && !failed)
        {
            if (Input.GetKeyDown(KeyCode.Escape)) isPaused = !isPaused;
            if (isPaused)
            {
                player.GetComponent<PlayerScript>().OnDisable();
                player.GetComponent<Rigidbody2D>().Sleep();
                if (timer.isTimerActive()) timer.toggleTimer();
                if ((Vector2)pauseScreen.position != pauseScreenAnchorPos)
                {
                    pauseScreen.position = Vector2.MoveTowards(pauseScreen.position, pauseScreenAnchorPos, 10f);
                    if (Vector2.Distance(pauseScreen.position, pauseScreenAnchorPos) <= 10f)
                        pauseScreen.position = pauseScreenAnchorPos;
                }
            }
            else
            {
                if (!timer.isTimerActive()) timer.toggleTimer();
                player.GetComponent<PlayerScript>().OnEnable();
                player.GetComponent<Rigidbody2D>().WakeUp();
                if ((Vector2)pauseScreen.position != pauseScreenAnchorPos + new Vector2(0, 1000f))
                {
                    pauseScreen.position = Vector2.MoveTowards(pauseScreen.position, pauseScreenAnchorPos + new Vector2(0, 1000f), 10f);
                    if (Vector2.Distance(pauseScreen.position, pauseScreenAnchorPos + new Vector2(0, 1000f)) <= 10f)
                        pauseScreen.position = pauseScreenAnchorPos + new Vector2(0, 1000f);
                }
            }
        }
        if (gameOver)
        {
            if ((Vector2)gameOverScreen.position != pauseScreenAnchorPos)
            {
                gameOverScreen.position = Vector2.MoveTowards(gameOverScreen.position, pauseScreenAnchorPos, 10f);
                if (Vector2.Distance(gameOverScreen.position, pauseScreenAnchorPos) <= 10f)
                   gameOverScreen.position = pauseScreenAnchorPos;
            }
        }
    }
    public void restartRun()
    {
        SceneManager.LoadScene(1);
    }
    public void startRewind()
    {
        player.GetComponent<PlayerScript>().startRewind();
        timer.startRewind();
        StartCoroutine(initiateRewindText());
    }
    public void resetLevel()
    {
        AugmentDataTransfer adt = GameObject.FindGameObjectWithTag("adt").GetComponent<AugmentDataTransfer>();
        adt.ids = new List<int>();
        for (int i = 0; i < lbm.buffs.Count; i++)
        {
            if (i < adt.ids.Count)
                adt.ids[i] = lbm.buffs[i].idNumber;
            else
                adt.ids.Add(lbm.buffs[i].idNumber);
            if (i < adt.rarityDisps.Count)
                adt.rarityDisps[i] = lbm.buffs[i].rarity;
            else
                adt.rarityDisps.Add(lbm.buffs[i].rarity);
            if (i < adt.negativeids.Count)
                adt.negativeids[i] = lbm.negatives[i].idNumber;
            else
                adt.negativeids.Add(lbm.negatives[i].idNumber);
        }
        if (!failed) adt.lastGhost = player.GetComponent<PlayerScript>().ghostRecording;
        adt.recordTime = timer.recordTime;
        //Debug.Log(timer.previousTime);
        adt.lives = timer.lives.getLives();
        adt.hasFailed = failed;
        SceneManager.LoadScene(2);
    }

    public void unPause()
    {
        isPaused = false;
    }

    public System.Collections.IEnumerator initiateRewindText()
    {
        rewindText.gameObject.SetActive(true);
        Image sprite = rewindText.transform.GetChild(0).GetComponent<Image>();
        sprite.sprite = rewindSpr;
        endScreen.gameObject.SetActive(false);
        int i = 0;
        while (true)
        {
            switch (i % 4)
            {
                case 0:
                    rewindText.text = "Rewinding";
                    break;
                case 1:
                    rewindText.text = "Rewinding.";
                    break;
                case 2:
                    rewindText.text = "Rewinding..";
                    break;
                case 3:
                    rewindText.text = "Rewinding...";
                    break;
            }
            i++;
            sprite.enabled = !sprite.enabled;
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void quitToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void endLevel()
    {
        endScreen.SetActive(true);
        StartCoroutine(endPause());
    }
    private System.Collections.IEnumerator endPause()
    {
        yield return new WaitForSeconds(2f);
        startRewind();
    }
}
