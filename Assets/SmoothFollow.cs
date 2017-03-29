 // Converted from UnityScript to C# at http://www.M2H.nl/files/js_to_c.php - by Mike Hergaarden


using UnityEngine;
using System.Collections;

public class SmoothFollow : MonoBehaviour {

    public GameObject player;       //Public variable to store a reference to the player game object
    public GameObject boundaries;

    private Vector3 offset;         //Private variable to store the offset distance between the player and camera

    // Use this for initialization
    void Start ()
    {
        //Calculate and store the offset value by getting the distance between the player's position and camera's position.
        offset = transform.position - player.transform.position;
    }

    // LateUpdate is called after Update each frame
    void LateUpdate ()
    {

        Bounds bounds = boundaries.GetComponent<SpriteRenderer>().bounds;
        float width = bounds.size.x;
        float height = bounds.size.y;



        Vector3 cameraLeftTop = Camera.main.ViewportToWorldPoint(new Vector3(0.0F, 0.0F, -10));
        Vector3 cameraRightBottom = Camera.main.ViewportToWorldPoint(new Vector3(1.0F, 1.0F, -10));

        float screenWidth = (cameraLeftTop.x - cameraRightBottom.x) / 2;
        float screenHeight = (cameraLeftTop.y - cameraRightBottom.y) / 2;

        // Set the position of the camera's transform to be the same as the player's, but offset by the calculated offset distance.
        Vector3 newPosition = player.transform.position + offset;
        transform.position = new Vector3(
          Mathf.Clamp(newPosition.x, -width / 2 + screenWidth, width / 2 - screenWidth),
          Mathf.Clamp(newPosition.y, -height / 2 + screenHeight, height / 2 - screenHeight),
          newPosition.z
        );
    }
}