using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class BuffType : MonoBehaviour
{
    [SerializeField] private string buffName;
    [SerializeField] [Range(0.01f,10f)] private float speedMultiplier = 1f;
    [SerializeField] [Range(0.01f,10f)] private float jumpMultiplier = 1f;
    [Tooltip("Set it lower to make things slippier (like you're on ice")]
    [SerializeField] [Range(0.01f,10f)] private float accelMultiplier = 1f;
    [SerializeField] [Range(0.01f,10f)] private float gravityMultiplier = 1f;
    [SerializeField] private bool dash = false;
    [Tooltip("If true, the buff will only be active for the interval")]
    [SerializeField] private bool intervalOnly = false;
    private bool isActive = false;
    private Image sprite;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sprite = GetComponent<Image>();
    }
    //apply buff to the player
    public void applyBuff(PlayerScript player, float interval)
    {
        isActive = true;
        Debug.Log(buffName + " activated!");
        sprite.color = Color.yellow;
        player.setMultipliers(speedMultiplier, jumpMultiplier,gravityMultiplier);
        if (intervalOnly) StartCoroutine(intervalBuffTimer(player, interval));
    }

    public void deactivate(PlayerScript player)
    {
        if (isActive)
        {
            sprite.color = Color.white;
            player.removeMultipliers(speedMultiplier, jumpMultiplier, gravityMultiplier);
            isActive = false;
        }   
    }

    private IEnumerator intervalBuffTimer(PlayerScript player, float interval)
    {
        yield return new WaitForSeconds(interval);
        deactivate(player);
    }
}
