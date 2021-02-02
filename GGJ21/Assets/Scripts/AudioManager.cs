using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public GameObject player;
    public GameObject monster;

    public AudioClip calm;
    public AudioClip tense;
    public AudioClip chasing;
    
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector2.Distance(player.transform.position, monster.transform.position) > 20)
        {
            if(audioSource.clip != calm)
            {
                audioSource.clip = calm;
                audioSource.volume = 0.2f;
                audioSource.Play();
            }
        }
        else if (Vector2.Distance(player.transform.position, monster.transform.position) < 20
                    && Vector2.Distance(player.transform.position, monster.transform.position) > 10)
        {
            if (audioSource.clip != tense)
            {
                audioSource.clip = tense;
                audioSource.Play();
            }
        }
        else if (Vector2.Distance(player.transform.position, monster.transform.position) < 10)
        {
            if (audioSource.clip != chasing)
            {
                audioSource.clip = chasing;
                audioSource.Play();
            }
        }
    }
}
