using System.Collections.Generic;
using UnityEngine;

public class LoopBuffMechanic : MonoBehaviour
{
    [SerializeField] private int numBuffs = 6;
    private GameObject player;
    private Timer timer;
    private float duration; //Total time (Last time record)
    private float intervals; //Time between buff activation
    private float time = 0f; //Float variable that keeps track of time lapsed
    private int tempInt = 0; //A variable that I'm using for testing
    [SerializeField] private List<BuffType> buffs = new List<BuffType>(); //List of different types of buffs
    private List<BuffType> buffsInPlay = new List<BuffType>(); //List of buffs that are in play

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //timer = GameObject.FindGameObjectWithTag("Timer").GetComponent<Timer>();
        duration = 14f;
        intervals = duration / numBuffs;
        for (int i = 0; i < numBuffs; i++)
        {
            Debug.Log(i);
            addBuffToList(i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time >= (intervals * (tempInt + 1)) && tempInt != numBuffs)
        {
            activateBuff(tempInt);
            tempInt++;
        }
    }

    void activateBuff(int id)
    {
        buffsInPlay[id].applyBuff();
    }
    //adds a buff from the list to the ones in play
    void addBuffToList(int id) {
        buffsInPlay.Add(buffs[id]);
    }
}
