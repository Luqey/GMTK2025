using System;
using System.Collections.Generic;
using NUnit.Framework;
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

    //Multipliers from buffs
    private float speedMult = 1f;
    private float jumpMult = 1f;

    float xSpeed;
    // onGround >= 0 = counts as on the ground
    int onGround;
    int jumpValueLastFrame;
    int jumpBuffer;
    // don't really need this, i just put this to be safe because sometimes cases can come up where unity can read multiple jump inputs when we only want one
    int jumpCooldown;
    int frameStartPressingJump;
    [SerializeField] float dashPower = 20f;
    private bool isDashing = false;

    // will break after 5965 hours of continuous playtime.
    // that's... a long time.... - Cherry
    int frameCounter;

    Rigidbody2D rigid;

    List<ghostPoint> ghostRecording;
    public GameObject ghost;
    bool alreadyRecorded;

    SpriteRenderer sprenderer;

    #region Luke's Animation Corner
    [SerializeField] Animator myAnim;
    #endregion

    void Awake()
    {
        controls = new InputSystem_Actions();
        rigid = GetComponent<Rigidbody2D>();
        ghostRecording = new List<ghostPoint>();
        sprenderer = GetComponent<SpriteRenderer>();

        myAnim = GetComponent<Animator>();
    }

    public void OnEnable()
    {
        move = controls.Player.Move;
        move.Enable();
        jump = controls.Player.Jump;
        jump.Enable();
    }

    public void OnDisable()
    {
        move.Disable();
        jump.Disable();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        frameStartPressingJump = -1000;
        frameCounter = 0;
        alreadyRecorded = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(move.ReadValue<Vector2>());
    }

    void FixedUpdate()
    {
        onGround--;
        jumpBuffer--;
        jumpCooldown--;
        if (Physics2D.BoxCast(new Vector2(transform.position.x, transform.position.y) + new Vector2(0, -0.5f) + GetComponent<Collider2D>().offset, new Vector2(0.85f, 1), 0, Vector2.down, 0.1f, groundMask) && jumpCooldown < 0)
        {
            onGround = coyoteTimeFrames;
        }
        if (jump.ReadValue<float>() != 0 && jumpValueLastFrame == 0)
        {
            jumpBuffer = jumpBufferSize;
        }
        jumpValueLastFrame = (int)jump.ReadValue<float>();
        if (jump.ReadValue<float>() == 0)
        {
            frameStartPressingJump = -1000;
        }
        if (frameCounter - frameStartPressingJump < 32 && jump.ReadValue<float>() != 0)
        {
            rigid.linearVelocityY = (jumpPower * jumpMult) - (frameCounter - frameStartPressingJump) * 0.07f;
            jumpBuffer = -1;
            onGround = -1;
            jumpCooldown = 5;
        }
        if (onGround >= 0 && jumpBuffer >= 0 && jumpCooldown < 0)
        {
            frameStartPressingJump = frameCounter;
            rigid.linearVelocityY = jumpPower * jumpMult;
            jumpBuffer = -1;
            onGround = -1;
            jumpCooldown = 5;
        }
        switch ((int)(move.ReadValue<Vector2>().x * 1.5f))
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
                    if ((maxSpeed * speedMult) - xSpeed < accelRate * 0.01f && onGround >= 0)
                    {
                        xSpeed = maxSpeed * speedMult;
                    }
                    else if (xSpeed < maxSpeed * speedMult)
                    {
                        xSpeed += accelRate * 0.01f;
                    }
                    if (rigid.linearVelocityX == 0)
                    {
                        xSpeed = 0;
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
                    if ((maxSpeed * speedMult) - Mathf.Abs(xSpeed) < accelRate * 0.01f && onGround >= 0)
                    {
                        xSpeed = -(maxSpeed * speedMult);
                    }
                    else if (xSpeed > -(maxSpeed * speedMult))
                    {
                        xSpeed -= accelRate * 0.01f;
                    }
                    if (rigid.linearVelocityX == 0)
                    {
                        xSpeed = 0;
                    }
                }
                else
                {
                    xSpeed -= accelRate * 0.02f;
                }
                break;
        }
        rigid.linearVelocity = new Vector2(xSpeed + ((isDashing ? dashPower : 0) * (sprenderer.flipX ? -1 : 1)), rigid.linearVelocity.y);
        if (frameCounter % 5 == 0 && !alreadyRecorded)
        {
            ghostRecording.Add(new ghostPoint(transform.position, transform.eulerAngles, transform.localScale, sprenderer.sprite, !sprenderer.flipX));
        }
        frameCounter++;
        // Animation Stuff Below
        //myAnim.SetFloat("moveSpeed", xSpeed); 

        if (Math.Abs(xSpeed) > 0.1f)
        {
            myAnim.SetBool("isRunning", true);
            sprenderer.flipX = xSpeed < 0;
        }
        else
        {
            myAnim.SetBool("isRunning", false);
        }
    }

    public void recordingTest()
    {
        ghost.GetComponent<GhostScript>().points = ghostRecording;
        alreadyRecorded = true;
        transform.position = Vector2.zero;
        rigid.linearVelocity = Vector2.zero;
        ghost.GetComponent<GhostScript>().counter = 0;
    }
    public void recordGhost()
    {
        ghost.GetComponent<GhostScript>().points = ghostRecording;
        ghost.GetComponent<GhostScript>().counter = 0;
        ghostRecording = new List<ghostPoint>();
    }
    public void setMultipliers(float spMult, float jMult, float gMult)
    {
        speedMult *= spMult;
        jumpMult *= jMult;
        rigid.gravityScale *= gMult;
    }
    public void removeMultipliers(float spMult, float jMult, float gMult)
    {
        speedMult /= spMult;
        jumpMult /= jMult;
        rigid.gravityScale /= gMult;
    }
    public void setDash(bool dash)
    {
        isDashing = dash;
    }
}
