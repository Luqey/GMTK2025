using System.Collections;
using UnityEngine;

public class SpringScript : MonoBehaviour
{
    [SerializeField] private float launchPower = 20f;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void OnTriggerStay2D(Collider2D collider) {
        if (collider.gameObject.CompareTag("Player"))
        {
            if(collider.gameObject.GetComponent<Rigidbody2D>().linearVelocityY <= 0.01)
                collider.gameObject.GetComponent<PlayerScript>().springJump(launchPower);
            if (anim != null) StartCoroutine(springAnimation());
        }
    }
    IEnumerator springAnimation() {
        anim.SetBool("isTouched", true);
        yield return new WaitForSeconds(0.5f);
        anim.SetBool("isTouched", false);
    }
}
