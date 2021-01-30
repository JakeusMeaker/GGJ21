using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public float runSpeed = 20f;
    public float sprintSpeed = 5;

    public GameObject flashLight;

    public Vector3[] flashlightPositions; // 0 = down , 1 = left, 2 = top, 3 = right

    [SerializeField]private Sprite defaultSprite;

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

        if(horizontal > 0.5f) // right
        {
            sr.flipX = true;
            anim.SetBool("Sideways", true);
            anim.SetBool("Vertical", false);
            flashLight.transform.eulerAngles = flashlightPositions[0];
        }
        else if(horizontal < -0.5f) //left
        {
            sr.flipX = false;
            anim.SetBool("Sideways", true);
            anim.SetBool("Vertical", false);
            flashLight.transform.eulerAngles = flashlightPositions[1];
        }
        else if(vertical > 0.5f) //up
        {
            sr.flipY = true;
            anim.SetBool("Vertical", true);
            anim.SetBool("Sideways", false);
            flashLight.transform.eulerAngles = flashlightPositions[2];
        }
        else if (vertical < -0.5f) //down
        {
            sr.flipY = false;
            anim.SetBool("Vertical", true);
            anim.SetBool("Sideways", false);
            flashLight.transform.eulerAngles = flashlightPositions[3];
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

        if (Input.GetButton("Sprint"))
        {
            rb.velocity = new Vector2(horizontal * sprintSpeed, vertical * sprintSpeed);
        }
        else
        {
            rb.velocity = new Vector2(horizontal * runSpeed, vertical * runSpeed);
        }        
    }
}
