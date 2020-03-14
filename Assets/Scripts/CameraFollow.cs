using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    //the object to match Y position of
    public Transform target;

    // top limit and bottom limit beyond which camera can't go
    public float topLimit = 10.0f;
    public float bottomLimit = -10.0f;

    //how quickly should camera follow the target
    public float followSpeed = 0.5f;

    private void LateUpdate()
    {
        if (target != null)
        {
            Vector3 newPosition = this.transform.position;

            newPosition.y = Mathf.Lerp(newPosition.y, target.transform.position.y, followSpeed);

            //clamp newPosition within limits
            newPosition.y = Mathf.Min(newPosition.y, topLimit);
            newPosition.y = Mathf.Max(newPosition.y, bottomLimit);

            transform.position = newPosition;
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

    //draw a line from topLimit to bottomLimit when selected in editr
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 topPoint = new Vector3(this.transform.position.x, topLimit, this.transform.position.z);
        Vector3 bottomPoint = new Vector3(this.transform.position.x, bottomLimit, this.transform.position.z);

        Gizmos.DrawLine(topPoint, bottomPoint);
    }
}
