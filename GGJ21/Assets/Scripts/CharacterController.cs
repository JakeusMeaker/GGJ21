using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public float runSpeed = 20f;

    [SerializeField] Sprite defaultSprite;

    private float horizontal;
    private float vertical;
    private float moveLimiter = 0.7f;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        sr.sprite = defaultSprite;
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        if(horizontal > 0.5f)
        {
            sr.flipX = true;
            anim.SetBool("Sideways", true);
            anim.SetBool("Vertical", false);
        }
        else if(horizontal < -0.5f)
        {
            sr.flipX = false;
            anim.SetBool("Sideways", true);
            anim.SetBool("Vertical", false);
        }
        else if(vertical > 0.5f)
        {
            sr.flipY = true;
            anim.SetBool("Vertical", true);
            anim.SetBool("Sideways", false);
        }
        else if (vertical < -0.5f)
        {
            sr.flipY = false;
            anim.SetBool("Vertical", true);
            anim.SetBool("Sideways", false);
        }
        else
        {
            sr.flipX = false;
            sr.flipY = false;
            anim.SetBool("Sideways", false);
            anim.SetBool("Vertical", false);
            sr.sprite = defaultSprite;
        }

    }

    private void FixedUpdate()
    {
        if (horizontal != 0 && vertical != 0)
        {
            horizontal *= moveLimiter;
            vertical *= moveLimiter;
        }

        rb.velocity = new Vector2(horizontal * runSpeed, vertical * runSpeed);
    }
}
