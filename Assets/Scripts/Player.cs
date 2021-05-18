using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D playerRB;

    public Transform groundCheck;

    private float horizontal, vertical;
    public float speed;
    public float jumpAccel;

    public bool grounded;
    public bool backwards;
    public bool atacking;

    public int idAnimation;

    public Collider2D standing, crouching;


    // Start is called before the first frame update
    void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        grounded = Physics2D.OverlapCircle(groundCheck.position, 0.02f);
        playerRB.velocity = new Vector2(horizontal * speed, playerRB.velocity.y);
    }

    void Flip()
    {
        backwards = !backwards;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }

    public void IsAtacking(int atack)
    {
        atacking = false;
        if(atack != 0)
        {
            atacking = true;
        }
    }
    // Update is called once per frame
    void Update()
    {
        HandleGetAxisInput();

        HandlePlayerShouldFlip();

        HandleChangeAnimation();

        HandlePlayerShouldAtack();

        HandlePlayerShouldJump();

        HandlePlayerShouldLockOnAtack();

        HandleHitBox();

        HandlePlayerVelocity();

        HandleAnimation();
    }

    private void HandleGetAxisInput()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
    }

    private void HandlePlayerShouldFlip()
    {
        if (horizontal > 0 && backwards == true)
        {
            Flip();
        }
        else if (horizontal < 0 && backwards == false)
        {
            Flip();
        }
    }

    private void HandleChangeAnimation()
    {
        if (vertical < 0)
        {
            idAnimation = 2;
            if (grounded == true)
            {
                horizontal = 0;
            }
        }
        else if (horizontal != 0)
        {
            idAnimation = 1;
        }
        else
        {
            idAnimation = 0;
        }
    }

    private void HandlePlayerShouldAtack()
    {
        if (Input.GetButtonDown("Fire1") && vertical >= 0 && atacking == false)
        {
            animator.SetTrigger("atack");
        }
    }

    private void HandlePlayerShouldJump()
    {
        if (Input.GetButtonDown("Jump") && grounded == true && atacking == false)
        {
            playerRB.AddForce(new Vector2(0, jumpAccel));
        }
    }
    
    private void HandlePlayerShouldLockOnAtack()
    {
        if (atacking == true && grounded == true)
        {
            horizontal = 0;
        }
    }

    private void HandleHitBox()
    {
        crouching.enabled = false;
        standing.enabled = true;
        if (vertical < 0 && grounded == true)
        {
            crouching.enabled = true;
            standing.enabled = false;
        }
    }

    private void HandlePlayerVelocity()
    {
        playerRB.velocity = new Vector2(horizontal * speed, playerRB.velocity.y);
    }

    private void HandleAnimation()
    {
        animator.SetBool("grounded", grounded);
        animator.SetInteger("idAnimation", idAnimation);
        animator.SetFloat("speedY", playerRB.velocity.y);
    }
}
