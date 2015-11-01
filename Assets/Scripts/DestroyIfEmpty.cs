using UnityEngine;
using System.Linq;
using System.Collections;

public class DestroyIfEmpty : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
        if((from Transform t in transform select t).Count() == 0)
        {
            Destroy(gameObject);
        }
	}
}
