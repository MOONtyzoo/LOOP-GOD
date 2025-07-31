using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private float followSpeed;
    [SerializeField] private float followMultiplier = 1f;

    private void Update()
    {
        SmoothFollowPlayerY();
    }

    private void SmoothFollowPlayerY()
    {
        float targetPosY = followMultiplier * player.transform.position.y;
        float newPosY = Mathf.Lerp(transform.position.y, targetPosY, followSpeed * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, newPosY, transform.position.z);
    }
}
