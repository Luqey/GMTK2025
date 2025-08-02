using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;
    private GameObject player;
    private Timer timer;
    [SerializeField] private RectTransform pauseScreen;
    [SerializeField] private GameObject endScreen;
    public Vector2 pauseScreenAnchorPos;
    public Vector2 initialPlayerPosition;
    //private InputAction menu;
    //InputSystem_Actions controls;
    public LoopBuffMechanic lbm;
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
        //controls = new InputSystem_Actions();
    }

    void Start()
    {
        player = FindFirstObjectByType<PlayerScript>().gameObject;
        initialPlayerPosition = player.transform.position;
        timer = FindFirstObjectByType<Timer>();
        pauseScreenAnchorPos = pauseScreen.position;
        pauseScreen.position += new Vector3(0, 1000f, 0);
        //menu = controls.Player.Menu;
        //menu.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if (!endScreen.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Escape)) isPaused = !isPaused;
            if (isPaused)
            {
                player.GetComponent<PlayerScript>().OnDisable();
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
                player.GetComponent<PlayerScript>().OnEnable();
                if (!timer.isTimerActive()) timer.toggleTimer();
                if ((Vector2)pauseScreen.position != pauseScreenAnchorPos + new Vector2(0, 1000f))
                {
                    pauseScreen.position = Vector2.MoveTowards(pauseScreen.position, pauseScreenAnchorPos + new Vector2(0, 1000f), 10f);
                    if (Vector2.Distance(pauseScreen.position, pauseScreenAnchorPos + new Vector2(0, 1000f)) <= 10f)
                        pauseScreen.position = pauseScreenAnchorPos + new Vector2(0, 1000f);
                }
            }
        }
    }
    public void restartRun()
    {
        SceneManager.LoadScene(1);
    }
    public void startRewind()
    {
        Debug.Log("dumbtest0");
        player.GetComponent<PlayerScript>().startRewind();
        timer.startRewind();
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
        adt.lastGhost = player.GetComponent<PlayerScript>().ghostRecording;
        adt.recordTime = timer.recordTime;
        adt.previousTime = timer.previousTime;
        Debug.Log(timer.previousTime);
        adt.lives = timer.lives.getLives();
        SceneManager.LoadScene(1);
        // player.transform.position = initialPlayerPosition;
        // player.GetComponent<PlayerScript>().OnEnable();
        // timer.resetTimer();
        // if (!timer.isTimerActive()) timer.toggleTimer();
        // endScreen.SetActive(false);
    }

    public void unPause()
    {
        isPaused = false;
    }
}
