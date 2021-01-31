using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoorScript : MonoBehaviour
{
    Text text;

    SceneChanger levelManager;

    private void Start()
    {
        text = GameObject.FindGameObjectWithTag("PlayerUI").GetComponent<Text>();
        levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<SceneChanger>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name == "Player")
        {
            bool hasKey = collision.GetComponent<CharacterController>().pickedUpExitKey;

            if (hasKey)
            {
                text.text = "You unlocked the door";
                if (Input.GetButton("Jump"))
                {
                    //win
                    Debug.Log("Freedom");
                    levelManager.WinScene();
                }
            }
            else if (!hasKey)
            {
                text.text = "You need a key";
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        text.text = "";
    }
}
