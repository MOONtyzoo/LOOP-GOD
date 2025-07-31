using System.Collections.Generic;
using UnityEngine;

public class TrackScroller : MonoBehaviour
{
    [SerializeField] private List<GameObject> trackPrefabs;
    [SerializeField] private GameObject emptyTrackPrefab;
    [SerializeField] private float trackWidth;

    [SerializeField, Tooltip("The X position at which a track object will be destroyed and cause a new track object to be created at the front.")]
    private float trackThresholdX;
    [SerializeField] private float initialTrackOffset;

    private GameObject leftTrack;
    private GameObject rightTrack;

    private float scrollDistance;
    private float trackOffset;

    private void Awake()
    {
        scrollDistance = 0.0f;
        trackOffset = initialTrackOffset;
        leftTrack = SpawnEmptyTrack();
        rightTrack = SpawnRandomTrack();
        PositionTracks();
    }

    private void Update()
    {
        scrollDistance = GameManager.Instance.GetDistance();
        if (IsTrackThesholdPassed())
        {
            SpawnNewTrack();
        }
        PositionTracks();
    }

    private void PositionTracks()
    {
        leftTrack.transform.position = new Vector2(trackOffset - scrollDistance, leftTrack.transform.position.y);
        rightTrack.transform.position = new Vector2(leftTrack.transform.position.x + trackWidth, rightTrack.transform.position.y);
    }

    private bool IsTrackThesholdPassed() => leftTrack.transform.position.x < trackThresholdX;

    private void SpawnNewTrack()
    {
        Destroy(leftTrack);
        leftTrack = rightTrack;
        rightTrack = SpawnRandomTrack();
        trackOffset += trackWidth;
    }

    private GameObject SpawnEmptyTrack()
    {
        return Instantiate(emptyTrackPrefab, transform);
    }

    private GameObject SpawnRandomTrack()
    {
        GameObject randomTrackPrefab = trackPrefabs[Random.Range(0, trackPrefabs.Count)];
        return Instantiate(randomTrackPrefab, transform);
    }
}
