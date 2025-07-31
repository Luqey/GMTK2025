using UnityEngine;
using UnityEngine.InputSystem;

public class DragAndDropAugment : MonoBehaviour
{
    bool inHand;
    public Camera mainCamera;
    float mouseInputLastFrame;
    public InputSystem_Actions controls;
    InputAction click;
    public LayerMask UImask;
    public int rarity;
    public int id;
    public string description;
    public GameObject rarityDisplay;
    public Sprite[] rarityLabels;


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
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        inHand = true;
        rarityDisplay.GetComponent<SpriteRenderer>().sprite = rarityLabels[rarity];
    }

    // Update is called once per frame
    void Update()
    {
        if (inHand)
        {
            transform.position = mainCamera.ScreenToWorldPoint(new Vector3(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y, 5));
            if (click.ReadValue<float>() != 0 && mouseInputLastFrame == 0)
            {
                Collider2D hit = Physics2D.OverlapCircle(transform.position, 3.0f, UImask);
                Debug.Log(hit);
                if (hit && hit.gameObject.tag == "augment slot")
                {
                    inHand = false;
                    hit.gameObject.GetComponent<AugmentSlot>().Populate(rarity, id, description);
                    gameObject.SetActive(false);
                }
            }
        }
        
        mouseInputLastFrame = click.ReadValue<float>();
    }
}
