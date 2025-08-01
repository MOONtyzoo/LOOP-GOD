using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private float followSpeed;
    [SerializeField] private float followMultiplier = 1f;
    [SerializeField] private float tiltStrength;
    [SerializeField] private float tiltSmoothSpeed;

    private new Camera camera;

    private float currentTilt = 0f;

    private void Awake()
    {
        camera = GetComponent<Camera>();
    }

    private void Update()
    {
        SmoothFollowPlayerY();
        Tilt();
    }

    private void SmoothFollowPlayerY()
    {
        float targetPosY = followMultiplier * player.transform.position.y;
        float newPosY = Mathf.Lerp(transform.position.y, targetPosY, followSpeed * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, newPosY, transform.position.z);
    }

    private void Tilt()
    {
        float trackMultiplier = player.GetTrack() - 2;
        float rotationX = transform.rotation.eulerAngles.x;
        float rotationY = transform.rotation.eulerAngles.y;
        float targetTilt = trackMultiplier * tiltStrength;
        currentTilt = Mathf.Lerp(currentTilt, targetTilt, tiltSmoothSpeed * Time.deltaTime);
        Quaternion newRotation = Quaternion.Euler(rotationX, rotationY, currentTilt);
        transform.rotation = newRotation;
    }
}
