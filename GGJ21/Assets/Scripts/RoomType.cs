using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomType : MonoBehaviour
{
    public int type;

    public GameObject[] doorways;

    public Transform keySpawnLocation;

    public void RoomDestruction()
    {
        Destroy(gameObject);
    }
}
