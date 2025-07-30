using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class LoopBuffMechanic : MonoBehaviour
{
    [SerializeField] private int numBuffs = 6;
    private float duration; //Total time (Last time record)
    private float intervals; //Time between buff activation
    private float time = 0f; //Float variable that keeps track of time lapsed
    private int tempInt = 0; //A variable that I'm using for testing
    private List<string> buffs = new List<string>(); //Change datatype later

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        duration = 14f;
        intervals = duration / numBuffs;
        for (int i = 0; i < numBuffs; i++)
        {
            buffs.Add("Buff " + i + " activated");
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
        Debug.Log(buffs[id]);
    }
}
