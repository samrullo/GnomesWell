using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    //the rope segment prefab to use
    public GameObject ropeSegmentPrefab;

    //a list of rope segment objects
    List<GameObject> ropeSegments = new List<GameObject>();

    //are we currently extending or retracting the rope
    public bool isIncreasing { get; set; }
    public bool isDecreasing { get; set; }

    // the rigidbody that the end of the rope should be connected to
    public Rigidbody2D gnomeRightLeg;

    // the maximum length a rope segment should be if we need to extedn the rope more than this create a new rope segment
    public float maxRopeSegmentLength = 1.0f;

    //how quickly we should pay out a new rope segment
    public float ropeSpeed = 4.0f;

    // line renderer that actually renders the rope
    LineRenderer lineRenderer;



    // Start is called before the first frame update
    void Start()
    {
        //cache line renderer so that we don't have to get each frame
        lineRenderer = GetComponent<LineRenderer>();

        // reset the rope so that we are good to go
        ResetRope();
        
    }

    // remove all rope segments and create a new one
    void ResetRope()
    {
        foreach(GameObject ropeSegment in ropeSegments)
        {
            Destroy(ropeSegment);
        }

        ropeSegments = new List<GameObject>();
        CreateRopeSegment();
    }

    // attaches a new rope segment at the top of the rope
    void CreateRopeSegment()
    {
        // create a new rope segment
        GameObject segment = Instantiate(ropeSegmentPrefab, this.transform.position, Quaternion.identity);

        // make the new rope segment the child of the Rope object and make it keep its world positions
        segment.transform.SetParent(this.transform, true);

        // add it to the rope segments list
        ropeSegments.Insert(0, segment);

        Rigidbody2D segmentBody = segment.GetComponent<Rigidbody2D>();
        SpringJoint2D segmentJoint = segment.GetComponent<SpringJoint2D>();

        // raise error if the segment doesn't have either a rigidbody or springjoint
        if(segmentBody==null || segmentJoint == null)
        {
            Debug.LogError("Either rigidbody or springjoint is missing");
            return;
        }

        // now that we checked the segment has rigidbody and springjoint we can proceed
        // if this is the only rope segment it needs to be attached to the gnomeRightLeg
        if (ropeSegments.Count < 2)
        {
            SpringJoint2D gnomeRightLegJoint = gnomeRightLeg.GetComponent<SpringJoint2D>();
            gnomeRightLegJoint.connectedBody = segmentBody;
            gnomeRightLegJoint.distance = 0.1f;

            segmentJoint.distance = maxRopeSegmentLength;
        }
        else
        {
            // the new segment has to be attached to the next segment
            GameObject nextSegment = ropeSegments[1];
            SpringJoint2D nextSegmentJoint = nextSegment.GetComponent<SpringJoint2D>();
            nextSegmentJoint.connectedBody = segmentBody;
            segmentJoint.distance = 0.0f;
            
        }

        // finally connect the new segment to the Rope anchor
        segmentJoint.connectedBody = this.GetComponent<Rigidbody2D>();
    }

    void RemoveRopeSegment()
    {
        // if we don't have 2 or more segments stop
        if (ropeSegments.Count < 2)
        {
            return;
        }

        //else remove the toppest segment
        GameObject topSegment = ropeSegments[0];
        GameObject nextSegment = ropeSegments[1];

        // get SpringJoint2D component of the next segment so that we can change its connected body to the Rope anchor
        SpringJoint2D nextSegmentJoint = nextSegment.GetComponent<SpringJoint2D>();
        nextSegmentJoint.connectedBody = this.GetComponent<Rigidbody2D>();

        ropeSegments.RemoveAt(0);

        Destroy(topSegment);
    }

    // Update is called once per frame
    // every frame extend or retract the rope
    void Update()
    {
        //get the top segment and its joint
        GameObject topSegment = ropeSegments[0];
        SpringJoint2D topSegmentJoint = topSegment.GetComponent<SpringJoint2D>();

        if (isIncreasing)
        {
            
            // we are increasing the rope. First extend the top segment joint till it reaches
            // its maximum length and only then create a new rope segment
            if (topSegmentJoint.distance > maxRopeSegmentLength)
            {
                CreateRopeSegment();
            }
            else
            {
                topSegmentJoint.distance += ropeSpeed * Time.deltaTime;
            }

            
        }

        if (isDecreasing)
        {
            Debug.Log("isDecreasing is pressed, so will retract the rope");
            //when decreasing the rope we will remove the toppest segment if it has reached its minimum length
            // and then we connect the next segment to the rope anchor
            if (topSegmentJoint.distance <= 0.005f)
            {
                Debug.Log("top segment has reached minimum length so will remove a segment");
                RemoveRopeSegment();
            }
            else
            {
                Debug.Log("Decrementing top segment to reach minimum length");
                Debug.Log("current top segment join distance is "+topSegmentJoint.distance);
                topSegmentJoint.distance -= ropeSpeed * Time.deltaTime;
            }
        }

        // finally we will draw the rope using its line renderer component
        // line renderer draws a line from a collection of vertices
        // in our case we will need rope segments plus two more vertices one for the rope anchor another for the gnomeConnectedLeg
        if (lineRenderer != null)
        {
            lineRenderer.positionCount = ropeSegments.Count + 2;

            // top vertex is always the rope anchor
            lineRenderer.SetPosition(0, this.transform.position);

            for(int i = 0; i < ropeSegments.Count; i++)
            {
                lineRenderer.SetPosition(i + 1, ropeSegments[i].transform.position);
            }

            //last point is gnome's leg
            SpringJoint2D gnomeRightLegJoint = gnomeRightLeg.GetComponent<SpringJoint2D>();
            lineRenderer.SetPosition(ropeSegments.Count + 1, gnomeRightLeg.transform.TransformPoint(gnomeRightLegJoint.anchor));


        }
    }
}
