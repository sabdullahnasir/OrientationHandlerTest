using UnityEngine;
[System.Serializable]
public struct RectTransformData
{
    public Vector2 anchoredPosition;
    public Vector2 sizeDelta;
    public Vector2 anchorMin;
    public Vector2 anchorMax;
    public Vector2 pivot;
    public Quaternion rotation;
    public Vector3 localScale;
    public float leftOffsetMinX;
    public float rightOffsetMaxX;
    public float topOffsetMaxY;
    public float bottomOffsetMinY;
}


[System.Serializable]
public struct OrientationData
{
    public RectTransformData originalPosition;
    public RectTransformData portraitData;
    public RectTransformData landscapeLeftData;
    public RectTransformData landscapeRightData;

}