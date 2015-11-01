using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class CameraLink : MonoBehaviour {

    public Vector2 HorizontalRestriction = new Vector2();
    public Vector3 Offset = new Vector3(-2f, 0);

    public Transform t;

    private new Camera camera;
    private Bounds cameraBounds = new Bounds();

	// Use this for initialization
	void Start ()
    {
        camera = GetComponent<Camera>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        var cameraSize =
                camera.ScreenToWorldPoint(Camera.main.pixelWidth * Vector3.right + Camera.main.pixelHeight * Vector3.up)
                - camera.ScreenToWorldPoint(Vector3.zero);
        cameraSize.z = 0;

        var cameraBounds = new Bounds(camera.transform.position, cameraSize);
        cameraBounds.Expand(1f);

        var pos = transform.position;
        var y = Mathf.Lerp(pos.y, t.position.y, 10 * Time.deltaTime);

	    if(y > HorizontalRestriction.y)
        {
            y = HorizontalRestriction.y;
        }
        else if (y < HorizontalRestriction.x)
        {
            y = HorizontalRestriction.x;
        }
        
        pos.y = y;
        pos.x = t.position.x;
        transform.position = pos + Offset;
    }
}
