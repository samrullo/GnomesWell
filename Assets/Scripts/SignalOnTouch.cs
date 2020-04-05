using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]

public class SignalOnTouch : MonoBehaviour
{
    // The UnityEvent that triggers when we collide
    // Attach methods to run in the editor
    public UnityEvent onTouch;

    // if true play audio on touch
    public bool playAudioOnTouch;


    // when we enter a trigger area call SendSignal
    private void OnTriggerEnter2D(Collider2D collider)
    {
        SendSignal(collider.gameObject);
    }

    // when we collide with this object call SendSignal
    private void OnCollisionEnter2D(Collision2D collision)
    {
        SendSignal(collision.gameObject);
    }

    void SendSignal(GameObject objectThatHit)
    {
        if (objectThatHit.CompareTag("Player")){

            if (playAudioOnTouch)
            {
                var audio = GetComponent<AudioSource>();

                if(audio && audio.gameObject.activeInHierarchy)
                {
                    audio.Play();
                }
            }

            onTouch.Invoke();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
