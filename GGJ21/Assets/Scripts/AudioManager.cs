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
    
    AudioSource audio;

    private void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector2.Distance(player.transform.position, monster.transform.position) > 20)
        {
            if(audio.clip != calm)
            {
                audio.clip = calm;
                audio.volume = 0.2f;
                audio.Play();
            }
        }
        else if (Vector2.Distance(player.transform.position, monster.transform.position) < 20 
                    && Vector2.Distance(player.transform.position, monster.transform.position) > 10)
        {
            if (audio.clip != tense)
            {
                audio.clip = tense;
                audio.Play();
            }
        }
        if (Vector2.Distance(player.transform.position, monster.transform.position) < 7.5)
        {
            if (audio.clip != chasing)
            {
                audio.clip = chasing;
                audio.Play();
            }
        }
    }
}
