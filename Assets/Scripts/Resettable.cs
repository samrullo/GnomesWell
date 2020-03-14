using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Resettable : MonoBehaviour
{
    // In the editor connect this event to the methods that should run when the game resets
    public UnityEvent onReset;

    public void Reset()
    {
        //kicks off the event which calls all of the connected methods
        onReset.Invoke();
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
