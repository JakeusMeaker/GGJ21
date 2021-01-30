using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{
    public float runSpeed = 2f;
    public Transform targetPosition;

    [SerializeField] Sprite defaultSprite;

    private float horizontal;
    private float vertical;
    private float moveLimiter = 0.7f;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator anim;

    private Seeker seeker;
    private Path path;
    private float nextWaypointDistance = 3;
    private int currentWaypoint = 0;
    public bool reachedEndOfPath;


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
        if (path == null)
        {
            return;
        }

        reachedEndOfPath = false;

        float distanceToWaypoint;
        while (true)
        {
            distanceToWaypoint = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);
            if (distanceToWaypoint < nextWaypointDistance)
            {
                if (currentWaypoint + 1 < path.vectorPath.Count)
                {
                    currentWaypoint++;
                }
                else
                {
                    reachedEndOfPath = true;
                    break;
                }
            }
            else break;
        }

        float speedFactor = reachedEndOfPath ? Mathf.Sqrt(distanceToWaypoint / nextWaypointDistance) : 1f;

        Vector3 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;

        Vector2 velocity = dir * runSpeed * speedFactor;

        horizontal += velocity.x * Time.deltaTime;
        vertical += velocity.y * Time.deltaTime;

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

        Pathfind();
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

    public void StartPathfinding()
    {
        seeker = GetComponent<Seeker>();
        Pathfind();
    }

    void Pathfind()
    {
        seeker.StartPath(transform.position, targetPosition.position, OnPathComplete);
    }

    void OnPathComplete(Path p)
    {
        Debug.Log("Path Found" + p.error);

        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }
}
