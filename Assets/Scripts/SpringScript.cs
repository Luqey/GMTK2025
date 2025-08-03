using System.Collections;
using System.Security.Cryptography;
using UnityEngine;

public class SpringScript : MonoBehaviour
{
    [SerializeField] private float launchPower = 20f;
    [SerializeField] AudioClip[] springSfx;
    private AudioSource audioSource;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            if (collider.gameObject.GetComponent<Rigidbody2D>().linearVelocityY <= 0.01)
            {
                collider.gameObject.GetComponent<PlayerScript>().springJump(launchPower);
                playSfx();
            }
            if (anim != null) StartCoroutine(springAnimation());
        }
    }
    IEnumerator springAnimation()
    {
        anim.SetBool("isTouched", true);
        yield return new WaitForSeconds(0.5f);
        anim.SetBool("isTouched", false);
    }
    void playSfx()
    {
        if (springSfx.Length == 0) return;
        int rand = RandomNumberGenerator.GetInt32(0, springSfx.Length);
        audioSource.clip = springSfx[rand];
        audioSource.Play();
    }
}
