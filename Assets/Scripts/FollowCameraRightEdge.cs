using UnityEngine;

public class FollowCameraRightEdge : MonoBehaviour
{
    private void Update()
    {
        float newPosX = Camera.main.ScreenToWorldPoint(Vector2.right*Screen.width).x;
        transform.position = new Vector2(newPosX, transform.position.y);
    }
}
