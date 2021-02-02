using System.Collections;
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
    public GameObject key;

    public GameObject[] notes;

    public GameObject doorHorizontal;
    public GameObject doorVertical;

    public GameObject exitDoorHorizontal;
    public GameObject exitDoorVertical;

    public GameObject playerGO;
    public GameObject monsterGO;

    public Transform middle;

    public float minX;
    public float maxX;
    public float minY;

    public float gizmoRadius = 10;

    public LayerMask room;
    public LayerMask linecastColliders;

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

                CheckDoors(newRoom.GetComponent<RoomType>().doorways, true);

                SpawnItems();

                for (int i = 0; i < doorChecker.Count - 1; i++)
                {
                    if (doorChecker[i] != null)
                    {
                        GameObject[] doorways = doorChecker[i].GetComponent<RoomType>().doorways;
                        CheckDoors(doorways, false);
                    }
                }

                SetPlayerMonsterPositions();
                SpawnGraphGrid();
            }
        }
    }

    void SpawnItems()
    {
        int rndNo = Random.Range(0, doorChecker.Count);
        if (doorChecker[rndNo] != null)
        {
            RoomType keySpwnRoom = doorChecker[rndNo].GetComponent<RoomType>();
            key.transform.position = keySpwnRoom.keySpawnLocation.position;
        }


        for (int i = 0; i < notes.Length; i++)
        {
            while (true)
            {
                int newRndNo = Random.Range(0, doorChecker.Count);
                RoomType keySpwnRoom = doorChecker[newRndNo].GetComponent<RoomType>();
                if (doorChecker[newRndNo] != null && keySpwnRoom.keySpawnLocation != key.transform)
                {
                    notes[i].transform.position = keySpwnRoom.keySpawnLocation.position;
                    break;
                }
            }
        }
    }

    void CheckDoors(GameObject[] doorways, bool lastRoom)
    {
        for (int i = 0; i < doorways.Length; i++)
        {
            Debug.Log("Checking for door");
            //if (!Physics2D.OverlapCircle(doorways[i].transform.position, 50f, room))
            //{
            //    switch (doorways[i].name)
            //    {
            //        case "CheckPointLeft":
            //            Instantiate(doorHorizontal, doorways[i].transform);
            //            break;
            //        case "CheckPointRight":
            //            Instantiate(doorHorizontal, doorways[i].transform);
            //            break;
            //        case "CheckPointUp":
            //            Instantiate(doorVertical, doorways[i].transform);
            //            break;
            //        case "CheckPointDown":
            //            Instantiate(doorVertical, doorways[i].transform);
            //            break;
            //        default:
            //            break;
            //    }
            //}
            //else return;
            switch (doorways[i].name)
            {
                case "CheckPointLeft":
                    Vector2 startL = doorways[i].transform.position;
                    Vector2 endL = startL + new Vector2(-5, 0);
                    RaycastHit2D resultL = Physics2D.Linecast(startL, endL, linecastColliders);
                    if (resultL.collider == null)
                    {
                        if (!lastRoom)
                        {
                            Instantiate(doorHorizontal, doorways[i].transform);
                        }
                        else
                        {
                            Instantiate(exitDoorHorizontal, doorways[i].transform);
                        }
                    }
                    break;
                case "CheckPointRight":
                    Vector2 startR = doorways[i].transform.position;
                    Vector2 endR = startR + new Vector2(5, 0);
                    RaycastHit2D resultR = Physics2D.Linecast(startR, endR, linecastColliders);
                    if (resultR.collider == null)
                    {
                        if (!lastRoom)
                        {
                            Instantiate(doorHorizontal, doorways[i].transform);
                        }
                        else
                        {
                            Instantiate(exitDoorHorizontal, doorways[i].transform);
                        }
                    }
                    break;
                case "CheckPointUp":
                    Vector2 startU = doorways[i].transform.position;
                    Vector2 endU = startU + new Vector2(0, 5);
                    RaycastHit2D resultU = Physics2D.Linecast(startU, endU, linecastColliders);
                    if (resultU.collider == null)
                    {
                        if (!lastRoom)
                        {
                            Instantiate(doorVertical, doorways[i].transform);
                        }
                        else
                        {
                            Instantiate(exitDoorVertical, doorways[i].transform);
                        }
                    }
                    break;
                case "CheckPointDown":
                    Vector2 startD = doorways[i].transform.position;
                    Vector2 endD = startD + new Vector2(0, -5);
                    RaycastHit2D resultD = Physics2D.Linecast(startD, endD, linecastColliders);
                    if (resultD.collider == null)
                    {
                        if (!lastRoom)
                        {
                            Instantiate(doorVertical, doorways[i].transform);
                        }
                        else
                        {
                            Instantiate(exitDoorVertical, doorways[i].transform);
                        }
                    }
                    break;
                default:
                    break;
            }

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
        Gizmos.DrawWireSphere(transform.position, .5f);
    }
}
