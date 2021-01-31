﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class LevelGeneration : MonoBehaviour
{
    public Transform[] startPositions;
    public GameObject[] LRrooms;
    public GameObject[] LRDrooms;
    public GameObject[] LRUrooms;
    public GameObject[] LRUDrooms;
    public GameObject startRoom;
    public GameObject endRoom;

    public GameObject doorHorizontal;
    public GameObject doorVertical;

    public GameObject playerGO;
    public GameObject monsterGO;

    public Transform middle;

    public float minX;
    public float maxX;
    public float minY;

    public float gizmoRadius = 10;

    public LayerMask room;

    private int direction;
    private const float moveAmount = 10;

    [SerializeField]
    private Stack<GameObject> previousRooms = new Stack<GameObject>();
    [SerializeField] private List<GameObject> doorChecker = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        int randStartPos = Random.Range(0, startPositions.Length);
        transform.position = startPositions[randStartPos].position;
        GameObject newRoom = Instantiate(startRoom, transform.position, Quaternion.identity);
        previousRooms.Push(newRoom);
        doorChecker.Add(newRoom);

        direction = Random.Range(1, 6);
        Move();
    }

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
                doorChecker.Add(newRoom);

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
                doorChecker.Add(newRoom);

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
                        doorChecker.Add(newRoomGO);
                    }
                }

                Vector2 newPos = new Vector2(transform.position.x, transform.position.y - moveAmount);
                transform.position = newPos;

                int randTopRoom = Random.Range(0, LRUrooms.Length);
                GameObject newRoom = Instantiate(LRUrooms[randTopRoom], transform.position, Quaternion.identity);
                previousRooms.Push(newRoom);
                doorChecker.Add(newRoom);

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
                        SetPlayerMonsterPositions();
                    }
                }
                Move();
            }
            else
            {
                Debug.Log("Adding Exit room");
                transform.position = previousRooms.Peek().transform.position;
                doorChecker.Remove(doorChecker[doorChecker.Count - 1]);
                Destroy(previousRooms.Peek());
                previousRooms.Pop();
                GameObject newRoom = Instantiate(endRoom, transform.position, Quaternion.identity);
                previousRooms.Push(newRoom);
                doorChecker.Add(newRoom);

                for (int i = 0; i < doorChecker.Count - 1; i++)
                {
                    if (doorChecker[i] != null)
                    {
                        GameObject[] doorways = doorChecker[i].GetComponent<RoomType>().doorways;
                        CheckDoors(doorways);
                    }
                }

                SetPlayerMonsterPositions();
                SpawnGraphGrid();
            }
        }
    }

    void CheckDoors(GameObject[] doorways)
    {
        for (int i = 0; i < doorways.Length; i++)
        {
            Debug.Log("Checking for door");
            if (!Physics2D.OverlapCircle(doorways[i].transform.position, .5f, room))
            {
                switch (doorways[i].name)
                {
                    case "CheckPointLeft":
                        Instantiate(doorHorizontal, doorways[i].transform);
                        break;
                    case "CheckPointRight":
                        Instantiate(doorHorizontal, doorways[i].transform);
                        break;
                    case "CheckPointUp":
                        Instantiate(doorVertical, doorways[i].transform);
                        break;
                    case "CheckPointDown":
                        Instantiate(doorVertical, doorways[i].transform);
                        break;
                    default:
                        break;
                }
            }
            else return;
        }
    }

    void SetPlayerMonsterPositions()
    {
        monsterGO.transform.position = previousRooms.Peek().transform.position;

        for (int i = previousRooms.Count; i > 3; i--)
        {
            if (previousRooms != null)
            {
                EnemyAI enemy = monsterGO.GetComponent<EnemyAI>();
                enemy.patrolRooms.Add(previousRooms.Peek());
                previousRooms.Pop();
            }
        }

        for (int i = previousRooms.Count; i > 1; i--)
        {
            doorChecker.Add(previousRooms.Peek());
            previousRooms.Pop();
        }

        playerGO.transform.position = previousRooms.Peek().transform.position;
    }

    void SpawnGraphGrid()
    {
        AstarData data = AstarPath.active.data;

        GridGraph gridGraph = data.gridGraph;

        int width = 50;
        int height = 50;
        float nodeSize = 1;

        gridGraph.center = middle.position;

        gridGraph.SetDimensions(width, height, nodeSize);


        Debug.Log("Rescanning");
        AstarPath.active.Scan(gridGraph);

        StartMonsterAI();
    }

    void StartMonsterAI()
    {
        Debug.Log("Starting Monster");
        monsterGO.GetComponent<EnemyAI>().StartPathfinding();
    }

    void OnDrawGizmos()
    {
        // Display the explosion radius when selected
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, gizmoRadius);
    }
}
