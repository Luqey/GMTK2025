using UnityEngine;

public class BuffType : MonoBehaviour
{
    [SerializeField] private string buffName;
    [SerializeField] private float speedChange;
    [SerializeField] private float jumpHeightChange;
    private SpriteRenderer sprite;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    //apply buff to the player
    public void applyBuff()
    {
        Debug.Log(buffName + " activated!");
        sprite.color = Color.yellow;
    }
}
