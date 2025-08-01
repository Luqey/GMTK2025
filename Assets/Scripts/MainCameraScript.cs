using JetBrains.Annotations;
using UnityEngine;

public class MainCameraScript : MonoBehaviour
{

    public GameObject player;
    [SerializeField] float xOffset = 0f;
    [SerializeField] float yOffset = 0f;
    Vector2 vel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.position += new Vector3(vel.x * Time.deltaTime + xOffset, vel.y * Time.deltaTime + yOffset, 0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        vel = new Vector2(((player.transform.position - transform.position) * 0.1f).x, ((player.transform.position - transform.position) * 0.1f).y);
        transform.position = transform.position + (player.transform.position - transform.position) * 0.1f;
        transform.position = new Vector3(transform.position.x + xOffset, transform.position.y + yOffset, -10);
    }
}
