using System;
using System.Xml.XPath;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    public InputSystem_Actions controls;
    InputAction move;
    InputAction jump;

    [Tooltip("In unity units/sec")]
    public float maxSpeed;
    [Tooltip("In unity units/sec^2")]
    public float accelRate;
    [Tooltip("Not really in any specific units")]
    public float jumpPower;

    [Tooltip("In physics frames, which are 1/100th of a second")]
    public int coyoteTimeFrames;
    [Tooltip("In physics frames, which are 1/100th of a second")]
    public int jumpBufferSize;
    public LayerMask groundMask;

    float xSpeed;
    // onGround >= 0 = counts as on the ground
    int onGround;
    int jumpValueLastFrame;
    int jumpBuffer;
    // don't really need this, i just put this to be safe because sometimes cases can come up where unity can read multiple jump inputs when we only want one
    int jumpCooldown;
    int frameStartPressingJump;

    // will break after 5965 hours of continuous playtime.
    int frameCounter;

    Rigidbody2D rigid;

    void Awake()
    {
        controls = new InputSystem_Actions();
        rigid = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        move = controls.Player.Move;
        move.Enable();
        jump = controls.Player.Jump;
        jump.Enable();
    }

    void OnDisable()
    {
        move.Disable();
        jump.Disable();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        frameStartPressingJump = -1000;
        frameCounter = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        onGround--;
        jumpBuffer--;
        jumpCooldown--;
        if (Physics2D.BoxCast(new Vector2(transform.position.x, transform.position.y) + new Vector2(0, -0.5f), new Vector2(1, 1), 0, Vector2.down, 0.1f, groundMask) && jumpCooldown < 0)
        {
            onGround = coyoteTimeFrames;
        }
        if (jump.ReadValue<float>() != 0 && jumpValueLastFrame == 0)
        {
            jumpBuffer = jumpBufferSize;
        }
        jumpValueLastFrame = (int)jump.ReadValue<float>();
        if (frameCounter - frameStartPressingJump < 32 && jump.ReadValue<float>() != 0)
        {
            rigid.linearVelocityY = 10.5f - (frameCounter - frameStartPressingJump) * 0.07f;
            jumpBuffer = -1;
            onGround = -1;
            jumpCooldown = 5;
        }
        if (onGround >= 0 && jumpBuffer >= 0 && jumpCooldown < 0)
        {
            frameStartPressingJump = frameCounter;
            rigid.linearVelocityY = 10.5f;
            jumpBuffer = -1;
            onGround = -1;
            jumpCooldown = 5;
        }
        switch ((int)move.ReadValue<Vector2>().x)
        {
            case 0:
                if (Mathf.Abs(xSpeed) < accelRate * 0.01f)
                {
                    xSpeed = 0;
                }
                else
                {
                    if (xSpeed > 0)
                    {
                        xSpeed -= accelRate * 0.01f;
                    }
                    else
                    {
                        xSpeed += accelRate * 0.01f;
                    }
                }
                break;
            case 1:
                if (xSpeed > 0)
                {
                    if (maxSpeed - xSpeed < accelRate * 0.01f)
                    {
                        xSpeed = maxSpeed;
                    }
                    else if (xSpeed < maxSpeed)
                    {
                        xSpeed += accelRate * 0.01f;
                    }
                }
                else
                {
                    xSpeed += accelRate * 0.02f;
                }
                break;
            case -1:
                if (xSpeed < 0)
                {
                    if (maxSpeed - Mathf.Abs(xSpeed) < accelRate * 0.01f)
                    {
                        xSpeed = -maxSpeed;
                    }
                    else if (xSpeed > -maxSpeed)
                    {
                        xSpeed -= accelRate * 0.01f;
                    }
                }
                else
                {
                    xSpeed -= accelRate * 0.02f;
                }
                break;
        }
        rigid.linearVelocity = new Vector2(xSpeed, rigid.linearVelocity.y);
        frameCounter++;
    }
}
