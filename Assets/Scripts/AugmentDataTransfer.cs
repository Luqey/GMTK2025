using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AugmentDataTransfer : MonoBehaviour
{
    public List<int> ids;
    public List<int> rarityDisps;
    public List<int> negativeids;
    public List<ghostPoint> lastGhost;
    public float recordTime;
    public float previousTime;
    public bool overrideSceneBuffs;
    public int lives;
    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("adt");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
        if (ids.Count < 1)
        {
            for (int i = 0; i < 6; i++)
            {
                ids.Add(-1);
                rarityDisps.Add(-1);
                negativeids.Add(-1);
            }
        }
        overrideSceneBuffs = false;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1 && overrideSceneBuffs)
        {
            LoopBuffMechanic lbm = FindFirstObjectByType<LoopBuffMechanic>();
            lbm.setBuffList(ids, rarityDisps, negativeids);
            lbm.player.ghost.GetComponent<GhostScript>().points = lastGhost;
            lbm.player.ghost.GetComponent<GhostScript>().counter = 0;
            lbm.timer.previousTime = previousTime;
            lbm.timer.previousTimeText.text = lbm.timer.updateTime(previousTime);
            lbm.timer.recordTime = recordTime;
            lbm.timer.recordTimeText.text = lbm.timer.updateTime(recordTime);
            lbm.timer.lives.setLives(lives);
            lbm.setDuration(previousTime);
            overrideSceneBuffs = false;
            Debug.Log(previousTime);
        }
    }
}
