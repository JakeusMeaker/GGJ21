using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoteReader : MonoBehaviour
{
    Text noteText;
    Text UIText;

    public GameObject notePanel;
    public NoteSO so;

    bool reading = false;

    private void Start()
    {
        noteText = GameObject.FindGameObjectWithTag("Notes").GetComponent<Text>();
        UIText = GameObject.FindGameObjectWithTag("PlayerUI").GetComponent<Text>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name == "Player")
        {
            UIText.text = "Read note?";
            if (Input.GetButton("Jump") && !reading)
            {
                Debug.Log("spacepressed");
                reading = true;
                notePanel.SetActive(true);
                Time.timeScale = 0;
                noteText.text = so.noteText;
            }
            else if (Input.GetButtonUp("Jump") && reading)
            {
                reading = false;
                notePanel.SetActive(false);
                Time.timeScale = 1;
                noteText.text = "";
            }
        }
    }
}
