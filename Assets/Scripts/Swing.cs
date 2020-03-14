using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swing : MonoBehaviour
{

    public float swingSensitivity = 100.0f;

    private void FixedUpdate()
    {
        // if rigidbody doesn't exist anymore remove this component
        if (GetComponent<Rigidbody2D>() == null)
        {
            Destroy(this);
            return;
        }

        float swing = InputManager.instance.sidewaysMotion;
        Vector2 force = new Vector2(swing * swingSensitivity, 0);
        //Debug.Log("will add " + force + " to the gnome");
        GetComponent<Rigidbody2D>().AddForce(force);
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
