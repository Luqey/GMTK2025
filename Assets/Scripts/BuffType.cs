using System.Collections;
using UnityEngine;

public class BuffType : MonoBehaviour
{
    [SerializeField] private string buffName;
    [Tooltip("Be careful not to set this to 0; it will make the player unable to move")]
    [SerializeField] private float speedMultiplier = 1f;
    [Tooltip("Be careful not to set this to 0; it will make the player unable to jump")]
    [SerializeField] private float jumpMultiplier = 1f;
    [Tooltip("If true, the buff will only be active for the interval")]
    [SerializeField] private bool intervalOnly = false;
    private bool isActive = false;
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
    public void applyBuff(PlayerScript player, float interval)
    {
        isActive = true;
        Debug.Log(buffName + " activated!");
        sprite.color = Color.yellow;
        player.setMultipliers(speedMultiplier, jumpMultiplier);
        if (intervalOnly) StartCoroutine(intervalBuffTimer(player, interval));
    }

    public void deactivate(PlayerScript player)
    {
        if (isActive)
        {
            sprite.color = Color.white;
            player.removeMultipliers(speedMultiplier, jumpMultiplier);
            isActive = false;
        }   
    }

    private IEnumerator intervalBuffTimer(PlayerScript player, float interval)
    {
        yield return new WaitForSeconds(interval);
        deactivate(player);
    }
}
