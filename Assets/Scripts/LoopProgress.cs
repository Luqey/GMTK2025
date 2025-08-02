using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoopProgress : MonoBehaviour
{
    public List<Sprite> sprites;
    public LoopBuffMechanic lbm;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (lbm.duration > 0 && !lbm.player.isRewinding && lbm.player.canMove && lbm.time > lbm.duration)
            GetComponent<Image>().sprite = sprites[(int)((lbm.time / lbm.duration) * 40)];
    }
}
