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

    bool near = false;

    private void Start()
    {
        noteText = GameObject.FindGameObjectWithTag("Notes").GetComponent<Text>();
        UIText = GameObject.FindGameObjectWithTag("PlayerUI").GetComponent<Text>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump") && !reading && near)
        {
            Debug.Log("spacepressed");
            reading = true;
            notePanel.SetActive(true);
            noteText = GameObject.FindGameObjectWithTag("Notes").GetComponent<Text>();
            noteText.text = so.noteText;
        }
        else if (Input.GetButtonUp("Jump") && reading)
        {
            reading = false;
            noteText.text = "";
            notePanel.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name == "Player")
        {
            UIText.text = "Read note?";
            near = true;
        }
        else
        {
            near = false;
        }
    }
}
