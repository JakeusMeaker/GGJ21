using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{
    public float runSpeed = 20f;
    public Transform targetPosition;

    [SerializeField] Sprite defaultSprite;

    private float horizontal;
    private float vertical;
    private float moveLimiter = 0.7f;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator anim;
    private Seeker seeker;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        seeker = GetComponent<Seeker>();
        sr.sprite = defaultSprite;

        seeker.StartPath(transform.position, targetPosition.position, OnPathComplete);
    }

    // Update is called once per frame
    void Update()
    {
        if (horizontal > 0.5f)
        {
            sr.flipX = true;
            anim.SetBool("Sideways", true);
            anim.SetBool("Vertical", false);
        }
        else if (horizontal < -0.5f)
        {
            sr.flipX = false;
            anim.SetBool("Sideways", true);
            anim.SetBool("Vertical", false);
        }
        else if (vertical > 0.5f)
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

    public void StartPathfinding()
    {
        
    }

    void Wander()
    {

    }

    void ChasePlayer()
    {

    }

    void OnPathComplete(Path p)
    {
        Debug.Log("Path Found" + p.error);
    }
}
