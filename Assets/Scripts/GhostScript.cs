using System.Collections.Generic;
using UnityEngine;

public class GhostScript : MonoBehaviour
{
    public List<ghostPoint> points;
    public int counter;
    SpriteRenderer sprenderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        counter = 0;
        sprenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        if (FindFirstObjectByType<PlayerScript>().canMove)
        {
            if (points != null && counter / 5 < points.Count - 1)
            {
                if (counter % 5 == 0)
                {
                    transform.position = points[counter / 5].position;
                    transform.eulerAngles = points[counter / 5].eulerAngles;
                    transform.localScale = points[counter / 5].scale;
                    sprenderer.sprite = points[counter / 5].sprite;
                    sprenderer.flipX = !points[counter / 5].facingRight;
                }
                else
                {
                    transform.position = points[counter / 5].position + (points[counter / 5 + 1].position - points[counter / 5].position) * ((counter % 5) / 5.0f);
                    transform.eulerAngles = points[counter / 5].eulerAngles + (points[counter / 5 + 1].eulerAngles - points[counter / 5].eulerAngles) * ((counter % 5) / 5.0f);
                    transform.localScale = points[counter / 5].scale + (points[counter / 5 + 1].scale - points[counter / 5].scale) * ((counter % 5) / 5.0f);
                }
            }
            counter++;
        }   
    }
}
