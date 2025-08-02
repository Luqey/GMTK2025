using System;
using System.Collections.Generic;
using Unity.Mathematics;
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
    private int counter = 0;
    private bool isCrouched = false;
    private bool cannotStand = false; //used for checking if the player can stand up
    [Tooltip("Multiplies with acceleration rate, keep it very small")]
    [SerializeField] float slideDecelerationRate = 0.001f; 
    [SerializeField] private float dashPower = 20f;
    private bool isDashing = false;
    private int jumpCount = 1;
    private int jumpsMade = 0;
    private bool invert = false;

    // will break after 5965 hours of continuous playtime.
    // that's... a long time.... - Cherry
    int frameCounter;

    Rigidbody2D rigid;
    private bool isRewinding = false;
    private int rewindCount = 0;
    public List<ghostPoint> ghostRecording;
    private Stack<ghostPoint> rewindRecording;
    private ghostPoint rewindPoint;
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
        rewindRecording = new Stack<ghostPoint>();
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
        if (!isRewinding)
        {
            onGround--;
            jumpBuffer--;
            jumpCooldown--;
            if (Physics2D.BoxCast(new Vector2(transform.position.x, transform.position.y) + new Vector2(0, -0.5f) + GetComponent<Collider2D>().offset, new Vector2(0.85f, 1), 0, Vector2.down, 0.1f, groundMask) && jumpCooldown < 0)
            {
                onGround = coyoteTimeFrames;
                jumpsMade = 0;
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
            if ((onGround >= 0 || jumpsMade < jumpCount) && jumpBuffer >= 0 && jumpCooldown < 0 && !isCrouched && !cannotStand)
            {
                myAnim.Play("JumpUp");
                myAnim.SetBool("isFalling", false);
                frameStartPressingJump = frameCounter;
                rigid.linearVelocityY += jumpPower * jumpMult;
                jumpBuffer = -1;
                onGround = -1;
                jumpCooldown = 5;
                jumpsMade++;
            }
            switch ((int)(move.ReadValue<Vector2>().y * 1.5f))
            {
                case 1:
                    isCrouched = invert;
                    break;
                case 0:
                    isCrouched = false;
                    break;
                case -1:
                    isCrouched = !invert;
                    break;
            }
            if (isCrouched)
            {
                cannotStand = Physics2D.BoxCast(new Vector2(transform.position.x, transform.position.y) + new Vector2(0, 0.5f) + GetComponent<Collider2D>().offset, new Vector2(0.85f, 1), 0, Vector2.down, 0.1f, groundMask);
                myAnim.SetBool("isSliding", true);
                gameObject.GetComponent<CapsuleCollider2D>().size = new Vector2(2f, 1f);
                gameObject.GetComponent<CapsuleCollider2D>().direction = CapsuleDirection2D.Horizontal;
                gameObject.GetComponent<CapsuleCollider2D>().offset = new Vector2(0, -1.05f);
                if (Mathf.Abs(xSpeed) < accelRate * 0.01f)
                {
                    xSpeed = 0;
                }
                else
                {
                    //myAnim.Play("Slide");
                    xSpeed += accelRate * slideDecelerationRate * (xSpeed > 0 ? -1 : 1);
                }
            }
            else if (!cannotStand)
            {
                myAnim.SetBool("isSliding", false);
                gameObject.GetComponent<CapsuleCollider2D>().size = new Vector2(1f, 2f);
                gameObject.GetComponent<CapsuleCollider2D>().direction = CapsuleDirection2D.Vertical;
                gameObject.GetComponent<CapsuleCollider2D>().offset = new Vector2(0, -0.55f);
                switch ((int)(move.ReadValue<Vector2>().x * 1.5f) * (invert ? -1 : 1))
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
                            if (Mathf.Abs(rigid.linearVelocityX) < 0.1f)
                            {
                                xSpeed = 0;
                            }
                        }
                        else if (xSpeed == 0)
                        {
                            if ((maxSpeed * speedMult) - xSpeed < accelRate * 0.01f && onGround >= 0)
                            {
                                xSpeed = maxSpeed * speedMult;
                            }
                            else if (xSpeed < maxSpeed * speedMult)
                            {
                                xSpeed += accelRate * 0.01f;
                            }
                        }
                        else
                        {
                            if (!isCrouched) xSpeed += accelRate * 0.02f;
                            if (Mathf.Abs(rigid.linearVelocityX) < 0.1f)
                            {
                                xSpeed = 0;
                            }
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
                            if (Mathf.Abs(rigid.linearVelocityX) < 0.1f)
                            {
                                xSpeed = 0;
                            }
                        }
                        else if (xSpeed == 0)
                        {
                            if ((maxSpeed * speedMult) - Mathf.Abs(xSpeed) < accelRate * 0.01f && onGround >= 0)
                            {
                                xSpeed = -(maxSpeed * speedMult);
                            }
                            else if (xSpeed > -(maxSpeed * speedMult))
                            {
                                xSpeed -= accelRate * 0.01f;
                            }
                        }
                        else
                        {
                            if (!isCrouched) xSpeed -= accelRate * 0.02f;
                            if (Mathf.Abs(rigid.linearVelocityX) < 0.1f)
                            {
                                xSpeed = 0;
                            }
                        }
                        break;
                }
            }
            else
            {
                if (gameObject.GetComponent<CapsuleCollider2D>().direction == CapsuleDirection2D.Horizontal)
                    cannotStand = Physics2D.BoxCast(new Vector2(transform.position.x, transform.position.y) + new Vector2(0, 0.5f) + GetComponent<Collider2D>().offset, new Vector2(0.85f, 1), 0, Vector2.down, 0.1f, groundMask);
                if (Mathf.Abs(xSpeed) < accelRate * 0.01f)
                {
                    xSpeed = 0;
                }
                else
                {
                    //myAnim.Play("Slide");
                    xSpeed += accelRate * slideDecelerationRate * (xSpeed > 0 ? -1 : 1);
                }
            }
            //If the player cannot stand and their velocity is 0, move them until they can stand
            if (cannotStand && xSpeed == 0)
            {
                xSpeed = 2f * (sprenderer.flipX ? -1 : 1);
            }
            if (Math.Abs(rigid.linearVelocityY) < 0.01f) rigid.linearVelocityY = 0;
            if (rigid.linearVelocityY < 0)
            {
                if (jumpsMade == 0) jumpsMade++;
                myAnim.SetBool("isFalling", true);
                myAnim.SetBool("hasLanded", false);
            }
            else if (rigid.linearVelocityY == 0 && onGround >= 0)
            {
                myAnim.SetBool("hasLanded", true);
                myAnim.SetBool("isFalling", false);
            }
            if (Math.Abs(xSpeed) > 0.1f && Math.Abs(rigid.linearVelocityX) > 0.1f)
            {
                myAnim.SetBool("isRunning", true);
                sprenderer.flipX = xSpeed < 0;
            }
            else
            {
                myAnim.SetBool("isRunning", false);
            }
            rigid.linearVelocity = new Vector2(xSpeed + ((isDashing ? dashPower : 0) * (sprenderer.flipX ? -1 : 1)), rigid.linearVelocity.y);
            if (frameCounter % 5 == 0 && !alreadyRecorded && move.enabled)
            {
                ghostRecording.Add(new ghostPoint(transform.position, transform.eulerAngles, transform.localScale, sprenderer.sprite, !sprenderer.flipX));
            }
            frameCounter++;
        }
        else
        {
            if (rewindRecording != null && counter / 2 < rewindCount - 1)
            {
                if (counter % 2 == 0)
                {
                    rewindPoint = rewindRecording.Pop();
                    transform.position = rewindPoint.position;
                    transform.eulerAngles = rewindPoint.eulerAngles;
                    transform.localScale = rewindPoint.scale;
                    sprenderer.sprite = rewindPoint.sprite;
                    sprenderer.flipX = !rewindPoint.facingRight;
                }
                else
                {
                    transform.position = rewindPoint.position + (rewindRecording.Peek().position - rewindPoint.position) * ((counter % 5) / 5.0f);
                    transform.eulerAngles = rewindPoint.eulerAngles + (rewindRecording.Peek().eulerAngles - rewindPoint.eulerAngles) * ((counter % 5) / 5.0f);
                    transform.localScale = rewindPoint.scale + (rewindRecording.Peek().scale - rewindPoint.scale) * ((counter % 5) / 5.0f);
                }
            }
            counter++;
            if (counter / 2 >= rewindCount - 1)
            {
                DataManager.instance.resetLevel();
            }
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
    public void setMultipliers(float spMult, float jMult, float aMult, float gMult)
    {
        speedMult *= spMult;
        jumpMult *= jMult;
        accelRate *= aMult;
        rigid.gravityScale *= gMult;
    }
    public void removeMultipliers(float spMult, float jMult, float aMult, float gMult)
    {
        speedMult /= spMult;
        jumpMult /= jMult;
        accelRate /= aMult;
        rigid.gravityScale /= gMult;
    }
    public void setDash(bool dash)
    {
        isDashing = dash;
    }
    public void addJump(int num)
    {
        jumpCount += num;
    }
    public void springJump(float springPower)
    {
        jumpsMade = 1;
        onGround = -1;
        jumpBuffer = -1;
        jumpCooldown = 5;
        rigid.linearVelocity += new Vector2(0, springPower);
        myAnim.Play("JumpUp");
    }

    public void startRewind()
    {
        isRewinding = true;
        foreach (ghostPoint g in ghostRecording)
        {
            rewindRecording.Push(g);
        }
        rewindCount = rewindRecording.Count;
        rigid.Sleep();
        myAnim.enabled = false;
    }

    public void invertControls(bool toggle)
    {
        invert = toggle;
    }
}
