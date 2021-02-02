using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Key : MonoBehaviour
{
    Text text;

    private void Start()
    {
        text = GameObject.FindGameObjectWithTag("PlayerUI").GetComponent<Text>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            text.text = "Picked up the key!";
            CharacterController player = collision.GetComponent<CharacterController>();
            player.pickedUpExitKey = true;
            StartCoroutine(Despawn());
        }
    }

    IEnumerator Despawn()
    {
        yield return new WaitForSeconds(0.5f);
        text.text = "";
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        
    }
}
