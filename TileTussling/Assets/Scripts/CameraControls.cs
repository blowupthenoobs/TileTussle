using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
    public Vector3 clickedPosition;
    private Vector3 newPos;
    private static Camera cam;

    public static Vector2 upperBounds;
    public static Vector2 lowerBounds;
    static float maxOrthoSize;

    void Awake()
    {
        cam = gameObject.GetComponent<Camera>();
    }
    
    void Update()
    {
        if(Input.GetMouseButtonDown(2))
            clickedPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if(Input.GetMouseButton(2))
        {
            newPos = (clickedPosition - Camera.main.ScreenToWorldPoint(Input.mousePosition));
            transform.position += newPos;
        }

        var scrollAmount = Input.mouseScrollDelta.y;
        if(cam.orthographicSize - scrollAmount > 0)
            cam.orthographicSize -= scrollAmount;

        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, 2, maxOrthoSize);
        // testPoint.transform.position = transform.position - new Vector3(cam.orthographicSize * cam.aspect, cam.orthographicSize);
        ClampPosition();

        if(Input.GetMouseButtonDown(0))
        {
            TileScript.clearTileEffects.Invoke();
            var clickPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            clickPoint += new Vector3(.5f, .5f);
            TileManagerScript.Instance.GetTile((int)clickPoint.x, (int)clickPoint.y)?.GetComponent<TileScript>().GetTilesOfDistance(2);
        }

        if(Input.GetKey(KeyCode.LeftShift))
            Hover();
        if(Input.GetKeyUp(KeyCode.LeftShift))
            TileScript.clearTileEffects.Invoke();
    }

    public void Hover()
    {   
        TileScript.clearTileEffects.Invoke();
        var mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePoint += new Vector3(.5f, .5f);
        TileManagerScript.Instance.GetTile((int)mousePoint.x, (int)mousePoint.y)?.GetComponent<TileScript>().MakeBlue();
    }

    public void ClampPosition()
    {
        Vector2 topCorner = transform.position + new Vector3(cam.orthographicSize * cam.aspect, cam.orthographicSize);
        Vector2 bottomCorner = transform.position - new Vector3(cam.orthographicSize * cam.aspect, cam.orthographicSize);

        Vector3 clampDist = new Vector2();
        if(topCorner.x > upperBounds.x)
            clampDist.x = upperBounds.x - topCorner.x;
        if(topCorner.y > upperBounds.y)
            clampDist.y = upperBounds.y - topCorner.y;
        if(bottomCorner.x < lowerBounds.x)
            clampDist.x = lowerBounds.x - bottomCorner.x;
        if(bottomCorner.y < lowerBounds.y)
            clampDist.y = lowerBounds.y - bottomCorner.y;
        
        transform.position = transform.position + clampDist;
    }

    public static void UpdateBounds()
    {
        var maxHeight = (upperBounds.y - lowerBounds.y) / 2;
        var maxWidth = (upperBounds.x - lowerBounds.x) / (cam.aspect * 2);

        if(maxHeight < maxWidth)
            maxOrthoSize = maxHeight;
        if(maxHeight > maxWidth)
            maxOrthoSize = maxWidth;
    }
}
