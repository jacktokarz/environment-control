using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private Transform player;
    private Camera mainCamera;

    public BoxCollider2D cameraBounds;
 
    private Vector3 min, max;
 
    bool isFollowing;
 
 
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        transform.position = new Vector3(player.position.x, player.position.y, transform.position.z);
    }
 
    void Start()
    {
        min = cameraBounds.bounds.min;
        max = cameraBounds.bounds.max;
        isFollowing = true;
        mainCamera = GetComponent<Camera>();
    }
 
    void Update()
    {
        var x = transform.position.x;
        var y = transform.position.y;
 
        if (isFollowing)
        {
            x = player.transform.position.x;
            y= player.transform.position.y;
        }
 
        // ortographicSize is the haldf of the height of the Camera.
        var cameraHalfWidth = mainCamera.orthographicSize * ((float)Screen.width / Screen.height);
 
        x = Mathf.Clamp(x, min.x + cameraHalfWidth, max.x - cameraHalfWidth);
        y = Mathf.Clamp(y, min.y + mainCamera.orthographicSize, max.y - mainCamera.orthographicSize);
 
        transform.position = new Vector3(x, y, transform.position.z);
    }
 
    // PixelPerfectScript.
    public static float RoundToNearestPixel(float unityUnits, Camera viewingCamera)
    {
        float valueInPixels = (Screen.height / (viewingCamera.orthographicSize * 2)) * unityUnits;
        valueInPixels = Mathf.Round(valueInPixels);
        float adjustedUnityUnits = valueInPixels / (Screen.height / (viewingCamera.orthographicSize * 2));
        return adjustedUnityUnits;
    }
 
    void LateUpdate()
    {
        Vector3 newPos = transform.position;
        Vector3 roundPos = new Vector3(RoundToNearestPixel(newPos.x, mainCamera), RoundToNearestPixel(newPos.y, mainCamera), newPos.z);
        transform.position = roundPos;
    }
 
    public void UpdateBounds()
    {
        min = cameraBounds.bounds.min;
        max = cameraBounds.bounds.max;
    }
}
