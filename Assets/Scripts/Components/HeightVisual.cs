using UnityEngine;

public class HeightVisual : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject VisualBody;
    [SerializeField] private GameObject VisualShadow;

    [Header("Visual")]
    [SerializeField] private float defaultOffsetY;
    [SerializeField] private float peakOffsetY;
    [SerializeField] private float defaultScale;
    [SerializeField] private float peakScale;
    [SerializeField] private float defaultShadowScale;
    [SerializeField] private float peakShadowScale;
    private float height = 0.0f;

    private void Update()
    {
        Animate();
    }

    private void Animate()
    {
        float jumpOffsetY = Mathf.Lerp(defaultOffsetY, peakOffsetY, height);
        float jumpScale = Mathf.Lerp(defaultScale, peakScale, height);
        float jumpShadowScale = Mathf.Lerp(defaultShadowScale, peakShadowScale, height);

        VisualBody.transform.localPosition = new Vector2(VisualBody.transform.localPosition.x, jumpOffsetY);
        VisualBody.transform.localScale = new Vector3(jumpScale, jumpScale, 1.0f);
        VisualShadow.transform.localScale = new Vector3(jumpShadowScale, jumpShadowScale, 1.0f);
    }

    public float GetHeight() => height;
    public void SetHeight(float newHeightVal) => height = newHeightVal; 
}
