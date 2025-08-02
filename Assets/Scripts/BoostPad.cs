using System.Collections;
using UnityEngine;

public class BoostPad : MonoBehaviour
{
    //[SerializeField] private float power = 100f;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerScript>().setDash(true);
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(endBoost(collision.GetComponent<PlayerScript>()));
        }
    }
    private IEnumerator endBoost(PlayerScript player)
    {
        yield return new WaitForSeconds(0.5f);
        player.setDash(false);
    }
}
