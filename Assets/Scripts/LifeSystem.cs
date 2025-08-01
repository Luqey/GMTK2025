using TMPro;
using UnityEngine;

public class LifeSystem : MonoBehaviour
{
    [SerializeField] private int lives = 3;
    [SerializeField] private TMP_Text lifeCount;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (lifeCount != null) lifeCount.text = lives.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (lives <= 0)
        {
            //Add gameover here
        }
    }

    public void changeLives(int amount)
    {
        lives += amount;
        lifeCount.text = lives.ToString();
    }
    public void setLives(int amount)
    {
        lives += amount;
        lifeCount.text = lives.ToString();
    }
    public int getLives()
    {
        return lives;
    }
}
