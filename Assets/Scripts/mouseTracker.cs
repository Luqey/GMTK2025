using UnityEngine;
using UnityEngine.InputSystem;

public class mouseTracker : MonoBehaviour
{
    public Camera mainCamera;
    public InputSystem_Actions controls;
    InputAction click;
    public LayerMask UImask;
    float mouseInputLastFrame;

    void Awake()
    {
        controls = new InputSystem_Actions();
    }

    void OnEnable()
    {
        click = controls.UI.Click;
        click.Enable();
    }

    void OnDisable()
    {
        click.Disable();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = mainCamera.ScreenToWorldPoint(new Vector3(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y, 5));
        Collider2D hit = Physics2D.OverlapCircle(transform.position, 3.0f, UImask);
        if (hit && hit.gameObject.GetComponent<AugmentOption>() != null)
        {
            hit.gameObject.GetComponent<AugmentOption>().framesToShowDescription = 5;
        }
        if (hit && hit.gameObject.GetComponent<AugmentSlot>() != null)
        {
            hit.gameObject.GetComponent<AugmentSlot>().show();
            hit.gameObject.GetComponent<AugmentSlot>().lastTouchingTime = Time.time;
        }
        if (click.ReadValue<float>() != 0 && mouseInputLastFrame == 0)
        {
            Debug.Log(hit);
            if (hit && hit.gameObject.GetComponent<AugmentOption>() != null)
            {
                hit.gameObject.GetComponent<AugmentOption>().clicked();
            }
        }
        mouseInputLastFrame = click.ReadValue<float>();
    }
}
