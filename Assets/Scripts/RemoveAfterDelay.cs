using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// remove an object after a certain delay
public class RemoveAfterDelay : MonoBehaviour
{
    // how many seconds to wait before removing
    public float delay = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        //kick off Remove coroutine
        StartCoroutine("Remove");
    }

    IEnumerator Remove()
    {
        // wait delay seconds and then destroy the gameObject attached to this object
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);

        // Don't say Destroy(this) - that just destroys RemoveAfterDelay script
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
