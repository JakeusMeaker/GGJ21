using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
    public Transform[] startPositions;
    public GameObject[] LRrooms;
    public GameObject[] LRDrooms;
    public GameObject[] LRUrooms;
    public GameObject[] LRUDrooms;

    public GameObject playerGO;
    public GameObject monsterGO;

    public float minX;
    public float maxX;
    public float minY;

    public float gizmoRadius = 10;

    public LayerMask room;

    private int direction;
    private const float moveAmount = 10;

    private float timeBetweenRoom;
    private float startTimeBetweemRoom = 0.001f;

    private bool stopGeneration;

    private Stack<GameObject> previousRooms = new Stack<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        int randStartPos = Random.Range(0, startPositions.Length);
        transform.position = startPositions[randStartPos].position;
        GameObject newRoom = Instantiate(LRrooms[0], transform.position, Quaternion.identity);
        previousRooms.Push(newRoom);

        direction = Random.Range(1, 6);
        Move();
    }

    //private void Update()
    //{
    //    if (timeBetweenRoom <= 0 && !stopGeneration)
    //    {
    //        Move();
    //        timeBetweenRoom = startTimeBetweemRoom;
    //    }
    //    else
    //    {
    //        timeBetweenRoom -= Time.deltaTime;
    //    }
    //}

    private void Move()
    {
        if (direction == 1 || direction == 2)
        {
            if (transform.position.x < maxX)
            {
                Vector2 newPos = new Vector2(transform.position.x + moveAmount, transform.position.y);
                transform.position = newPos;

                int randRoom = Random.Range(0, LRrooms.Length);
                GameObject newRoom = Instantiate(LRrooms[randRoom], transform.position, Quaternion.identity);
                previousRooms.Push(newRoom);

                direction = Random.Range(1, 6);
                if (direction == 3)
                {
                    if (transform.position.x < maxX)
                    {
                        direction = 2;
                    }
                    else
                    {
                        direction = 5;
                    }
                }
                else if (direction == 4)
                {
                    direction = 5;
                }
                Move();
            }
            else
            {
                direction = 5;
                Move();
            }
        }
        else if (direction == 3 || direction == 4)
        {
            if (transform.position.x > minX)
            {
                Vector2 newPos = new Vector2(transform.position.x - moveAmount, transform.position.y);
                transform.position = newPos;

                int randRoom = Random.Range(0, LRrooms.Length);
                GameObject newRoom = Instantiate(LRrooms[randRoom], transform.position, Quaternion.identity);
                previousRooms.Push(newRoom);

                direction = Random.Range(3, 6);
                if (transform.position.x > minX)
                {
                    direction = 3;
                }
                else
                {
                    direction = 5;
                }
                Move();
            }
            else
            {
                direction = 5;
                Move();
            }
        }
        else if (direction == 5)
        {
            if (transform.position.y > minY)
            {
                if (previousRooms.Count > 0)
                {
                    if (previousRooms.Peek().GetComponent<RoomType>().type != 1 &&
                            previousRooms.Peek().GetComponent<RoomType>().type != 3)
                    {
                        previousRooms.Peek().GetComponent<RoomType>().RoomDestruction();
                        int randBottomRoom = Random.Range(0, LRUDrooms.Length);
                        GameObject newRoomGO = Instantiate(LRUDrooms[randBottomRoom], transform.position, Quaternion.identity);
                        previousRooms.Push(newRoomGO);
                    }
                }

                Vector2 newPos = new Vector2(transform.position.x, transform.position.y - moveAmount);
                transform.position = newPos;

                int randTopRoom = Random.Range(0, LRUrooms.Length);
                GameObject newRoom = Instantiate(LRUrooms[randTopRoom], transform.position, Quaternion.identity);
                previousRooms.Push(newRoom);


                direction = Random.Range(1, 6);
                if (transform.position.x < maxX)
                {
                    direction = 2;
                }
                else
                {
                    direction = 5;
                }

                if (transform.position.x > minX)
                {
                    direction = 3;
                }
                else
                {
                    if (transform.position.y > minY)
                    {
                        direction = 5;
                    }
                    else
                    {
                        stopGeneration = true;
                    }
                }
                Move();
            }
            else
            {
                stopGeneration = true;
            }
        }
        
    }

    void OnDrawGizmos()
    {
        // Display the explosion radius when selected
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, gizmoRadius);
    }
}
