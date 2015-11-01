using UnityEngine;

using System.Linq;
using System.Collections.Generic;

//[ExecuteInEditMode]
public class AutoScroll : MonoBehaviour
{
    public enum ScrollDirection
    {
        Left,
        Right,
        Up,
        Down
    }

    public new Camera camera;
    public GameObject scrollable;

    public ScrollDirection Direction;
    public bool skipUpdate = false;

    private Bounds spriteBounds = new Bounds();
    //private Bounds cameraBounds = new Bounds();
    
	// Use this for initialization
	void Start ()
    {
        if (scrollable == null || camera == null)
            return;

        var cameraSize =
            camera.ScreenToWorldPoint(Camera.main.pixelWidth * Vector3.right + Camera.main.pixelHeight * Vector3.up)
            - camera.ScreenToWorldPoint(Vector3.zero);
        cameraSize.z = 0;

        var cameraBounds = new Bounds(camera.transform.position, cameraSize);
        cameraBounds.Expand(1f);

        var transforms = new List<Transform>(from Transform t in scrollable.transform select t);
        transforms.Add(scrollable.transform);

        spriteBounds = AddBounds(transforms, new Bounds());

        if (spriteBounds.size == Vector3.zero)
            return;

        switch (Direction)
        {
            case ScrollDirection.Left:
                break;

            case ScrollDirection.Right:

                if (spriteBounds.size.x == 0)
                    return;

                for (float x = cameraBounds.min.x; x < cameraBounds.max.x; x += spriteBounds.size.x)
                {
                    var spawnPosition = new Vector2(x + spriteBounds.size.x / 2, scrollable.transform.position.y);

                    var obj = GameObject.Instantiate(scrollable);
                    
                    obj.transform.position = spawnPosition;
                    obj.transform.parent = transform;

                    var pos = obj.transform.localPosition;
                    pos.y = spawnPosition.y;
                    obj.transform.localPosition = pos;
                }

                break;

            case ScrollDirection.Up:
                break;

            case ScrollDirection.Down:

                if (spriteBounds.size.y == 0)
                    return;

                for (float y = cameraBounds.min.y; y > cameraBounds.min.y; y -= spriteBounds.size.y)
                {
                    var spawnPosition = new Vector2(scrollable.transform.position.x, y + spriteBounds.size.y / 2);

                    var obj = GameObject.Instantiate(scrollable);

                    obj.transform.position = spawnPosition;
                    obj.transform.parent = transform;

                    var pos = obj.transform.localPosition;
                    pos.x = spawnPosition.x;
                    obj.transform.localPosition = pos;
                }

                break;

            default:
                break;
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        // If we don't have an object to scroll, we don't scroll at all
        if (skipUpdate || scrollable == null || camera == null || spriteBounds.size == Vector3.zero)
            return;

        var cameraSize =
            camera.ScreenToWorldPoint(Camera.main.pixelWidth * Vector3.right + Camera.main.pixelHeight * Vector3.up)
            - camera.ScreenToWorldPoint(Vector3.zero);
        cameraSize.z = 0;

        var cameraBounds = new Bounds(camera.transform.position, cameraSize);
        cameraBounds.Expand(1f);

        DestroyIfOutside(GetComponentsInChildren<SpriteRenderer>(), cameraBounds);
        objs.ForEach(obj => GameObject.Destroy(obj));
        
        var b = AddBounds(from Transform child in transform select child, new Bounds(transform.position, Vector3.zero));
        //var spawnPosition = Vector3.zero;
        
        switch (Direction)
        {
            case ScrollDirection.Left:
                break;

            case ScrollDirection.Right:

                //spawnPosition = b.max;

                //transform.Translate(-Vector2.right * parallax * Time.deltaTime);
                //if (spawnPosition.x > cameraBounds.max.x)
                //    return;

                if (spriteBounds.size.x == 0)
                    return;

                for (float x = b.max.x; x < cameraBounds.max.x; x += spriteBounds.size.x)
                {
                    var spawnPosition = new Vector2(x + spriteBounds.size.x / 2, scrollable.transform.position.y);

                    var obj = GameObject.Instantiate(scrollable);

                    obj.transform.position = spawnPosition;
                    obj.transform.parent = transform;

                    var pos = obj.transform.localPosition;
                    pos.y = spawnPosition.y;
                    obj.transform.localPosition = pos;
                }

                for (float x = b.min.x; x > cameraBounds.min.x; x -= spriteBounds.size.x)
                {
                    var spawnPosition = new Vector2(x - spriteBounds.size.x / 2, scrollable.transform.position.y);

                    var obj = GameObject.Instantiate(scrollable);

                    obj.transform.position = spawnPosition;
                    obj.transform.parent = transform;

                    var pos = obj.transform.localPosition;
                    pos.y = spawnPosition.y;
                    obj.transform.localPosition = pos;
                }

                //spawnPosition.x += spriteBounds.size.x / 2 - 0.1f;
                //spawnPosition.y = scrollable.transform.position.y;
                //spawnPosition.z = 0;
                break;

            case ScrollDirection.Up:
                break;

            case ScrollDirection.Down:

                if (spriteBounds.size.y == 0)
                    return;

                for (float y = cameraBounds.min.y; y > cameraBounds.min.y; y -= spriteBounds.size.y)
                {
                    var spawnPosition = new Vector2(scrollable.transform.position.x, y + spriteBounds.size.y / 2);

                    var obj = GameObject.Instantiate(scrollable);

                    obj.transform.position = spawnPosition;
                    obj.transform.parent = transform;

                    var pos = obj.transform.localPosition;
                    pos.x = spawnPosition.x;
                    obj.transform.localPosition = pos;
                }

                break;
        }

        //var spawn = GameObject.Instantiate(scrollable);
        //spawn.transform.position = spawnPosition;
        //spawn.transform.parent = transform;
    }

    public static Bounds AddBounds(IEnumerable<Transform> list, Bounds whenZero)
    {
        if (list == null || list.Count() <= 0)
            return whenZero;
        
        Bounds output = new Bounds(list.First().transform.position, Vector3.zero);

        foreach (var t in list)
        {
            var tList = from Transform child in t select child;

            if (tList.Count() > 0)
            {
                /**/
                var item = AddBounds(tList, whenZero);

                if (smallerV2(item.min, output.min))
                {
                    output.min = item.min;
                }

                if (biggerV2(item.max, output.max))
                {
                    output.max = item.max;
                }
                //*/
            }
            
            var srs = t.GetComponents<SpriteRenderer>();
            if (srs.Count() <= 0)
                continue;

            foreach (SpriteRenderer sr in t.GetComponents<SpriteRenderer>())
            {
                var item = sr.bounds;

                if (smallerV2(item.min, output.min))
                {
                    output.min = item.min;
                }

                if (biggerV2(item.max, output.max))
                {
                    output.max = item.max;
                }
            }
        }        

        //Debug.Log(output);
        return output;
    }

    private static bool smallerV2(Vector2 a, Vector2 b)
    {
        return a.x < b.x || a.y < b.y;
    }

    public static bool biggerV2(Vector2 a, Vector2 b)
    {
        return a.x > b.x || a.y > b.y;
    }

    public static List<GameObject> objs = new List<GameObject>();
    public void DestroyIfOutside(IEnumerable<SpriteRenderer> list, Bounds container)
    {
        foreach (var item in list)
        {
            var bnd = item.bounds;
            bool destroy = false;

            switch (Direction)
            {
                case ScrollDirection.Left:
                    destroy = bnd.min.x > container.max.x;
                    break;
                case ScrollDirection.Right:
                    destroy = bnd.max.x < container.min.x;
                    break;
                case ScrollDirection.Up:
                    destroy = bnd.max.y < container.min.y;
                    break;
                case ScrollDirection.Down:
                    destroy = bnd.min.y > container.max.y;
                    break;
            }

            if (destroy)
            {
                objs.Add(item.gameObject);
            }
        }
    }
}
