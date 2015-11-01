using UnityEngine;
using System.Collections;

public class Parallax : MonoBehaviour {

    public float parallaxMod = 1f;
    public new Camera camera;

    private Vector2 movement;

	// Use this for initialization
	void Start ()
    {
	    movement = camera.transform.position;
        //transform.position -= new Vector3(parallaxMod, 0);
	}
	
	// Update is called once per frame
	void Update ()
    {
        var parVector = movement;

        movement = camera.transform.position;
        parVector -= movement;

        transform.Translate(parVector * parallaxMod * Time.deltaTime * -1f);
    }
}
