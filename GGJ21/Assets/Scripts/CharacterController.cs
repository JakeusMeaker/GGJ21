using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public float runSpeed = 20f;
    public float sprintSpeed = 5;

    public GameObject flashLight;

    public Vector3[] flashlightRotations; // 0 = down , 1 = left, 2 = top, 3 = right
    public Vector3[] flashlightPositions;

    public bool pickedUpExitKey = false;

    public Sprite defaultSprite;

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
        flashLight.transform.localPosition = flashlightPositions[3];
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        if(horizontal > 0.5f) // right
        {
            anim.enabled = true;
            sr.flipX = true;
            sr.flipY = false;
            anim.SetBool("Sideways", true);
            anim.SetBool("Vertical", false);
            flashLight.transform.eulerAngles = flashlightRotations[0];
            flashLight.transform.localPosition = flashlightPositions[0];
        }
        else if(horizontal < -0.5f) //left
        {
            anim.enabled = true;
            sr.flipX = false;
            sr.flipY = false;
            anim.SetBool("Sideways", true);
            anim.SetBool("Vertical", false);
            flashLight.transform.eulerAngles = flashlightRotations[1];
            flashLight.transform.localPosition = flashlightPositions[1];
        }
        else if(vertical > 0.5f) //up
        {
            anim.enabled = true;
            sr.flipX = false;
            sr.flipY = true;
            anim.SetBool("Vertical", true);
            anim.SetBool("Sideways", false);
            flashLight.transform.eulerAngles = flashlightRotations[2];
            flashLight.transform.localPosition = flashlightPositions[2];
        }
        else if (vertical < -0.5f) //down
        {
            anim.enabled = true;
            sr.flipX = false;
            sr.flipY = false;
            anim.SetBool("Vertical", true);
            anim.SetBool("Sideways", false);
            flashLight.transform.eulerAngles = flashlightRotations[3];
            flashLight.transform.localPosition = flashlightPositions[3];
        }
        else
        {
            anim.enabled = false;
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
