using UnityEngine;
using System.Collections;

public class TransformConnector : MonoBehaviour {

    public Transform connectedTransform;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        var pos = transform.position;
        pos.x = connectedTransform.position.x;
        transform.position = pos;
	}
}
