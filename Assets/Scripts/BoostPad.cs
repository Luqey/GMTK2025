using System.Collections;
using UnityEngine;
using System.Security.Cryptography;

public class BoostPad : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip[] sfx;
    //[SerializeField] private float power = 100f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    void Update()
    {
        audioSource.volume = AudioManager.instance.updateVolume();
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playSfx();
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
    public void playSfx()
    {
        if (sfx.Length == 0) return;
        int rand = RandomNumberGenerator.GetInt32(0, sfx.Length);
        audioSource.clip = sfx[rand];
        audioSource.Play();
    }

}
