using NUnit.Framework;
using UnityEngine;

public class Train : MonoBehaviour
{
    public float speed = 5f;

    public float leftResetOffset = 2f;
    public float rightResetOffset = 1f;

    public bool movingRight = true;

    private Camera mainCamera;
    private float trainWidth;

    void Start()
    {
        mainCamera = Camera.main;

   
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            trainWidth = renderer.bounds.extents.x;
        }
    }

    void Update()
    {
        Vector2 direction = movingRight ? Vector2.right : Vector2.left;
        transform.Translate(direction * speed * Time.deltaTime);


        Vector3 viewportPos = mainCamera.WorldToViewportPoint(transform.position);

        if (movingRight)
        {

            if (viewportPos.x > 1 + (rightResetOffset / mainCamera.orthographicSize))
            {

                Vector3 leftEdge = mainCamera.ViewportToWorldPoint(new Vector3(0, 0.5f, transform.position.z - mainCamera.transform.position.z));
                transform.position = new Vector3(leftEdge.x - leftResetOffset, transform.position.y, transform.position.z);
            }
        }
        else
        {

            if (viewportPos.x < 0 - (leftResetOffset / mainCamera.orthographicSize))
            {

                Vector3 rightEdge = mainCamera.ViewportToWorldPoint(new Vector3(1, 0.5f, transform.position.z - mainCamera.transform.position.z));
                transform.position = new Vector3(rightEdge.x + rightResetOffset, transform.position.y, transform.position.z);
            }
        }
    }
}
