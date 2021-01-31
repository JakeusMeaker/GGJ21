using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoorScript : MonoBehaviour
{
    Text text;

    SceneChanger levelManager;

    bool near = false;

    private void Start()
    {
        text = GameObject.FindGameObjectWithTag("PlayerUI").GetComponent<Text>();
        levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<SceneChanger>();
    }

    private void Update()
    {
        if (Input.GetButton("Jump") && near)
        {
            //win
            Debug.Log("Freedom");
            levelManager.WinScene();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name == "Player")
        {
            bool hasKey = collision.GetComponent<CharacterController>().pickedUpExitKey;

            if (hasKey)
            {
                text.text = "You unlocked the door";
                near = true;
            }
            else if (!hasKey)
            {
                text.text = "You need a key";
                near = true;
            }
        }
        else
        {
            near = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        text.text = "";
    }
}
