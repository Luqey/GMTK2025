using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class LoopBuffMechanic : MonoBehaviour
{
    [SerializeField] private int numBuffs = 6;
    public PlayerScript player;
    public Timer timer;
    public float duration = 0f; //Total time (Last time record)
    private float intervals; //Time between buff activation
    public float time = 0f; //Float variable that keeps track of time lapsed
    private int buffId = 0;
    [SerializeField] public List<BuffType> buffs = new List<BuffType>(); //List of different types of buffs
    [SerializeField] public List<BuffType> negatives = new List<BuffType>(); //List of different types of negative buffs
    [SerializeField] private TMP_Text intervalText;
    [SerializeField] private Sprite[] augmentSprites;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = FindFirstObjectByType<PlayerScript>();
        timer = FindFirstObjectByType<Timer>();
        //This is for testing purposes, remove later
        /*for (int i = 0; i < numBuffs; i++)
        {
            addBuffToList(i, i);
        }
        */
        //addBuffToList(5, 1, 0);
        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer.isTimerActive() && duration != 0f)
        {
            time += Time.deltaTime;
            if (buffId >= 3)
            {
                intervalText.text = "Time Until Next:\n" + (int)(intervals * (buffId + 2) - time)  + " sec";
                if (time >= (intervals * (buffId + 2)) && buffId != numBuffs)
                {
                    activateBuff(buffId);
                    buffId++;
                }
            }
            else
            {
                 intervalText.text = "Time Until Next:\n" + (int)(intervals * (buffId + 1) - time)  + " sec";
                if (time >= (intervals * (buffId + 1)) && buffId != numBuffs)
                {
                    activateBuff(buffId);
                    buffId++;
                }
            }
        }
    }

    void activateBuff(int id)
    {
        if (id == 3)
        {
            if (buffs[id] != null) buffs[id].applyBuff(player, intervals * 2, augmentSprites);
            if (negatives[id] != null) negatives[id].applyBuff(player, intervals * 2,augmentSprites);
        }
        if (buffs[id] != null) buffs[id].applyBuff(player, intervals,augmentSprites);
        if (negatives[id] != null) negatives[id].applyBuff(player, intervals,augmentSprites);
    }
    //adds an augment from the list to the ones in play
    public void addBuffToList(int id, int r, int position)
    {
        if (position < numBuffs) buffs[position].setBuff(id, r);
        else Debug.Log("Position out of Range! Cannot add augment to list!");
    }
    //removes an augment from the loop
    public void removeBuffFromList(int position)
    {
        if (position < numBuffs) buffs[position].factoryReset();
        else Debug.Log("Position out of Range! Cannot remove augment that doesn't exist!");
    }

    public void setBuffList(List<int> ids, List<int> rs, List<int> ns)
    {
        for (int i = 0; i < ids.Count; i++)
        {
            buffs[i].setBuff(ids[i], rs[i]);
            negatives[i].setBuff(ns[i], -1);
        }
    }

    public void Reset()
    {
        foreach (BuffType b in buffs)
        {
            b.deactivate(player);
        }
        time = 0f;
        buffId = 0;
    }
    
    public void setDuration(float timeVar)
    {
        duration = timeVar;
        intervals = duration / 8;
    }
}
