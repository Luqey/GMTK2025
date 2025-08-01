using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;
    private GameObject player;
    private Timer timer;
    public Vector2 initialPlayerPosition;
    public LoopBuffMechanic lbm;
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
        AugmentDataTransfer adt = GameObject.FindGameObjectWithTag("adt").GetComponent<AugmentDataTransfer>();
        adt.ids = new List<int>();
        for (int i = 0; i < lbm.buffs.Count; i++)
        {
            if(i < adt.ids.Count)
                adt.ids[i] = lbm.buffs[i].idNumber;
            else
                adt.ids.Add(lbm.buffs[i].idNumber);
            if(i < adt.rarityDisps.Count)
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
        adt.lives = timer.lives.getLives();
        SceneManager.LoadScene(1);
        // player.transform.position = initialPlayerPosition;
        // player.GetComponent<PlayerScript>().OnEnable();
        // timer.resetTimer();
        // if (!timer.isTimerActive()) timer.toggleTimer();
        // endScreen.SetActive(false);
    }
}
