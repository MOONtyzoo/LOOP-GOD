using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private float followSpeed;

    private void Update()
    {
        SmoothFollowPlayerY();
    }

    private void SmoothFollowPlayerY()
    {
        float newPosY = Mathf.Lerp(transform.position.y, player.transform.position.y, followSpeed * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, newPosY, transform.position.z);
    }
}
