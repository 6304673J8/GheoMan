using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction { NONE, UP, DOWN, RIGHT, LEFT };

public class PlayerController : MonoBehaviour
{
    //Player Movement Basic Step
    private BoxCollider2D box2D;
    private Rigidbody2D rb2D;
    public Animator animator;
    private SpriteRenderer spriteRenderer;
    
    // Audio
    public AudioSource audioWeapon;
    public AudioSource audioStage;

    //Animator IDs
    private int runningID;
    private int jumpingID;
    private int shootingID;
    
    //Player Anim Variables
    private bool isRunning;
    private bool isShooting;
    private bool wasShooting;
    private bool isJumping;
    private bool hasJumped;
    private bool wasJumping;

    private float frameTime;
    private Direction whichDirection;
    private Vector2 actualSpeed;
    private Vector2 oldSpeed;
    public float walkSpeed = 1;
    public float jumpSpeed = 220;

    public GameObject bulletPrefab;
    public float offsetBullet;
    private float nextFire;
    // Start is called before the first frame update
    void Start()
    {
        box2D = GetComponent<BoxCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        runningID = Animator.StringToHash("isWalking");
        shootingID = Animator.StringToHash("isShooting");
        jumpingID = Animator.StringToHash("isJumping");
        isRunning = false;
        isShooting = false;
        isJumping = false;
        wasShooting = false;
        hasJumped = false;      //Used in order to know when the jump Stops
    }

    // Update is called once per frame
    void Update()
    {
        wasShooting = isShooting;
        isShooting = false;
        isRunning = false;
        whichDirection = Direction.NONE;

        if (Input.GetKey(KeyCode.X))
        {
            nextFire = Time.time;
            if (nextFire > 1)
            {
                isShooting = true;
                ShootingLogic();
            }
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            isRunning = true;
            whichDirection = Direction.RIGHT;
            spriteRenderer.flipX = false;
        } else if (Input.GetKey(KeyCode.LeftArrow))
        {
            spriteRenderer.flipX = true;
            isRunning = true;
            whichDirection = Direction.LEFT;
        }
        if (!isJumping)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isJumping = true;
                hasJumped = false;
            }
        }

        if (isRunning)
        {
            animator.SetBool(runningID, true);
        }
        else
        {
            animator.SetBool(runningID, false);
        }

        if(wasShooting != isShooting)
        {
            AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
            frameTime = state.normalizedTime - (int)state.normalizedTime;
            animator.SetBool(shootingID, isShooting);
            animator.Update(0);
            state = animator.GetCurrentAnimatorStateInfo(0);
            animator.Play(state.shortNameHash, 0, frameTime);
        }
        if(wasJumping != isJumping)
        {
            animator.SetBool(jumpingID, isJumping);
        }

        wasJumping = isJumping;
        oldSpeed = rb2D.velocity;
    }
    private void FixedUpdate()
    {
        float delta = Time.fixedDeltaTime * 1000;
        actualSpeed.y = rb2D.velocity.y;
        switch (whichDirection)
        {
            default:
                actualSpeed.x = 0;
                break;
            case Direction.RIGHT:
                actualSpeed.x = walkSpeed * delta;
                break;
            case Direction.LEFT:
                actualSpeed.x = -walkSpeed * delta;
                break;
        }
        rb2D.velocity = actualSpeed;
        if(isJumping && !hasJumped)
        {
            hasJumped = true;
            rb2D.AddForce(new Vector2(0, jumpSpeed * delta));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "CheckPoint")
        {
            //CheckPointController control = collision.gameObject.GetComponent<CheckPointController>();
            //control.startParticle();
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Scenario")
        {
            if (isJumping)
            {
                bool col1 = false;
                bool col2 = false;
                bool col3 = false;

                float center_x = (box2D.bounds.min.x + box2D.bounds.max.x) / 2;
                Vector2 centerPosition = new Vector2(center_x, box2D.bounds.min.y);
                Vector2 leftPosition = new Vector2(box2D.bounds.min.x + 0.1f, box2D.bounds.min.y);
                Vector2 rightPosition = new Vector2(box2D.bounds.max.x - 0.1f, box2D.bounds.min.y);

                RaycastHit2D[] hits = Physics2D.RaycastAll(centerPosition, -Vector2.up, 2);
                Debug.DrawRay(centerPosition, Vector2.down, Color.red, 1);
                if (checkRaycastWithScenario(hits)) { col1 = true; }

                hits = Physics2D.RaycastAll(leftPosition, -Vector2.up, 2);
                if (checkRaycastWithScenario(hits)) { col2 = true; }
                Debug.DrawRay(leftPosition, -Vector2.up, Color.red, 1);

                hits = Physics2D.RaycastAll(rightPosition, -Vector2.up, 2);
                if (checkRaycastWithScenario(hits)) { col3 = true; }
                Debug.DrawRay(rightPosition, -Vector2.up, Color.red, 1);

                if (col1 || col2 || col3) { isJumping = false; }
            }
        }
        /* In case I create Object to bounce
         * if (collision.gameObject.tag == "Body")
        {
            if (isJumping)
            {
                isJumping = false;
                jumpForce = 10;
                deathbodyToCrash = collision.gameObject;
            }
        }   */
    }

    private bool checkRaycastWithScenario(RaycastHit2D[] hits)
    {
        foreach(RaycastHit2D hit in hits)
        {
            if(hit.collider != null)
            {
                if(hit.collider.gameObject.tag == "Scenario")
                {
                    return true;
                }
            }
        }
        return false;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Scenario")
        {
            isJumping = true;
        }
    }

    public void ShootingLogic()
    {
        Vector2 pos = transform.right * offsetBullet + transform.position;

        nextFire = 0;
        GameObject bullet = Instantiate(bulletPrefab, pos, transform.rotation);
    }
}