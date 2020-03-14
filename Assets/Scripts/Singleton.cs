using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T>: MonoBehaviour where T:MonoBehaviour
{
    // This class allows other objects to refer to a single shared object.
    // GameManager and InputManager classes use this.
    // To use Singleton subclass it like so
    // public class MyManager : Singleton<MyManager>(){}
    // you can then access the single shared instance of the class
    // MyManager.instance.DoSomething();

    private static T _instance;

    // accessor. The first time this is called, _instance will be set up.
    // If an appropriate object can't be found error will be logged
    public static T instance
    {
        get
        {
            if (_instance == null)
            {
                //try to find the object
                _instance = FindObjectOfType<T>();

                //log if we can't find it
                if (_instance == null)
                {
                    Debug.LogError("Can't find " + typeof(T) + "!");
                }
            }
            return _instance;               
        }
    }

}
