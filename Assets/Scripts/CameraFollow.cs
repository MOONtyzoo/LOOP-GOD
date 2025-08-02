using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private float followSpeed;
    [SerializeField] private float followMultiplier = 1f;
    [SerializeField] private float tiltStrength;
    [SerializeField] private float tiltSmoothSpeed;

    [SerializeField] private float zoomDefault;
    [SerializeField] private float zoomDefaultOffsetX;
    [SerializeField] private float zoomStartSpeed;
    [SerializeField] private float zoomEndSpeed;
    [SerializeField] private float zoomFactor;
    [SerializeField] private float zoomOffsetFactorX;

    private new Camera camera;

    private float currentTilt = 0f;
    private float currentZoom;
    private float currentOffsetX;

    private void Awake()
    {
        camera = GetComponent<Camera>();

        currentZoom = zoomDefault;
        currentOffsetX = zoomDefaultOffsetX;
    }

    private void Update()
    {
        SmoothFollowPlayerY();
        Tilt();
        Zoom();
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

    private void Zoom()
    {
        float speed = GameManager.Instance.GetSpeed();
        float zoomLerpVal = Mathf.Clamp((speed - zoomStartSpeed) / (zoomEndSpeed - zoomStartSpeed), 0.0f, 1.0f);

        float targetZoom = zoomDefault + zoomLerpVal * zoomFactor;
        float newZoom = Mathf.Lerp(currentZoom, targetZoom, Time.deltaTime * tiltSmoothSpeed);
        currentZoom = newZoom;
        camera.orthographicSize = currentZoom;

        float targetX = zoomDefaultOffsetX + zoomLerpVal * zoomOffsetFactorX;
        float newOffsetX = Mathf.Lerp(currentOffsetX, targetX, Time.deltaTime * tiltSmoothSpeed);
        currentOffsetX = newOffsetX;
        transform.position = new Vector3(currentOffsetX, transform.position.y, transform.position.z);
    }
}
