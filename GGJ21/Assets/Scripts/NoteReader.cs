using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoteReader : MonoBehaviour
{
    Text UIText;

    public Text noteText;
    public Image notePanel;
    public NoteSO so;

    bool reading = false;

    bool near = false;

    private void Start()
    {
        UIText = GameObject.FindGameObjectWithTag("PlayerUI").GetComponent<Text>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump") && !reading && near)
        {
            Debug.Log("spacepressed");
            reading = true;
            noteText.text = so.noteText;
            notePanel.enabled = true;
        }
        else if (Input.GetButtonDown("Jump") && reading )
        {
            reading = false;
            noteText.text = "";
            notePanel.enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name == "Player")
        {
            UIText.text = "Read note?";
            near = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        UIText.text = "";
        near = false;
    }
}