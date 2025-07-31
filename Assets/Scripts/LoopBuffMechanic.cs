using System.Collections.Generic;
using UnityEngine;

public class LoopBuffMechanic : MonoBehaviour
{
    [SerializeField] private int numBuffs = 6;
    private PlayerScript player;
    private Timer timer;
    private float duration = 0f; //Total time (Last time record)
    private float intervals; //Time between buff activation
    private float time = 0f; //Float variable that keeps track of time lapsed
    private int buffId = 0;
    [SerializeField] private List<BuffType> buffs = new List<BuffType>(); //List of different types of buffs
    private BuffType[] buffsInPlay; //List of buffs that are in play

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        buffsInPlay = new BuffType[numBuffs];
        player = FindFirstObjectByType<PlayerScript>();
        timer = FindFirstObjectByType<Timer>();
        //This is for testing purposes, remove later
        for (int i = 0; i < numBuffs; i++)
        {
            addBuffToList(i, i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (timer.isTimerActive() && duration != 0f)
        {
            time += Time.deltaTime;
            if (time >= (intervals * (buffId + 1)) && buffId != numBuffs)
            {
                activateBuff(buffId);
                buffId++;
            }
        }
    }

    void activateBuff(int id)
    {
        if (buffsInPlay[id] != null) buffsInPlay[id].applyBuff(player, intervals);
    }
    //adds an augment from the list to the ones in play
    public void addBuffToList(int id, int position)
    {
        if (position < numBuffs) buffsInPlay[position] = buffs[id];
        else Debug.Log("Position out of Range! Cannot add augment to list!");
    }
    //removes an augment from the loop
    public void removeBuffFromList(int position)
    {
        if (position < numBuffs) buffsInPlay[position] = null;
        else Debug.Log("Position out of Range! Cannot remove augment that doesn't exist!");
    }

    public void Reset()
    {
        foreach (BuffType b in buffsInPlay)
        {
            b.deactivate(player);
        }
        time = 0f;
        buffId = 0;
    }
    
    public void setDuration(float timeVar)
    {
        duration = timeVar;
        intervals = duration / numBuffs;
    }
}
