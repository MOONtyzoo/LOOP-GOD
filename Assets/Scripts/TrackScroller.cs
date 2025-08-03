using System.Collections.Generic;
using UnityEngine;

public class TrackScroller : MonoBehaviour
{
    [SerializeField] private GameObject emptyTrackPrefab;
    [SerializeField] private float trackWidth;
    [SerializeField] private float trackSpawnPosY = 0.0f;
    [SerializeField] private float scrollMultiplier = 1.0f;

    [SerializeField, Tooltip("The X localPosition at which a track object will be destroyed and cause a new track object to be created at the front.")]
    private float trackThresholdX;
    [SerializeField] private float initialTrackOffset;

    private List<GameObject> trackPrefabs;

    private GameObject leftTrack;
    private GameObject rightTrack;

    private float scrollDistance;
    private float trackOffset;

    private void Awake()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        scrollDistance = 0.0f;
        trackOffset = initialTrackOffset;
        leftTrack = SpawnTrack(emptyTrackPrefab);
        rightTrack = SpawnRandomTrack();
        PositionTracks();
    }

    private void Update()
    {
        scrollDistance = scrollMultiplier * GameManager.Instance.GetDistance();
        if (IsTrackThesholdPassed())
        {
            SpawnNewTrack();
        }
        PositionTracks();
    }

    private void PositionTracks()
    {
        leftTrack.transform.localPosition = new Vector2(trackOffset - scrollDistance, leftTrack.transform.localPosition.y);
        rightTrack.transform.localPosition = new Vector2(leftTrack.transform.localPosition.x + trackWidth, rightTrack.transform.localPosition.y);
    }

    private bool IsTrackThesholdPassed() => leftTrack.transform.localPosition.x < trackThresholdX;

    private void SpawnNewTrack()
    {
        Destroy(leftTrack);
        leftTrack = rightTrack;
        rightTrack = SpawnRandomTrack();
        trackOffset += trackWidth;
    }

    private GameObject SpawnRandomTrack()
    {
        if (trackPrefabs == null || trackPrefabs.Count == 0) return SpawnTrack(emptyTrackPrefab);
        GameObject randomTrackPrefab = trackPrefabs[Random.Range(0, trackPrefabs.Count)];
        return SpawnTrack(randomTrackPrefab);
    }

    private GameObject SpawnTrack(GameObject trackPrefab)
    {
        GameObject newTrack = Instantiate(trackPrefab, transform);
        newTrack.transform.localPosition = new Vector2(newTrack.transform.localPosition.x, trackSpawnPosY);
        return newTrack;
    }

    public void SetTrackPrefabs(List<GameObject> newTrackPrefabs) => trackPrefabs = newTrackPrefabs;
}
