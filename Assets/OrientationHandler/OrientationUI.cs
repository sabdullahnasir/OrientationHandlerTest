using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class OrientationUI : MonoBehaviour
{

    public ScreenOrientationData orientationData;
    public ScreenOrientationData OrientationData
    {
        get { 

            return orientationData;
        }
        set { orientationData = value; }
    }
    private RectTransform rectTransform;
    public RectTransform RectTransform
    {
        get
        {
            if(rectTransform == null)
            {
                rectTransform = GetComponent<RectTransform>();
            }
             return rectTransform;
        }
        set { rectTransform = value; }
    }
    public enum OrientationUIType{Default,EventBased};
    public OrientationUIType orientationUIType;

    private void OnEnable()
    {
        if (Screen.orientation == ScreenOrientation.Portrait || Screen.orientation == ScreenOrientation.PortraitUpsideDown)
        {
            ApplyPortraitOrientation();
        }
        else if (Screen.orientation == ScreenOrientation.LandscapeLeft)
        {
            ApplyLandscapeLeftOrientation();
        }
        else
        {
            ApplyLandscapeRightOrientation();
        }
    }
    [ContextMenu("Portrait")]
    public void ApplyPortraitOrientation()
    {
        if (OrientationData != null && this.gameObject.activeInHierarchy && orientationUIType == OrientationUIType.Default)
        {
            RectTransformData rectTransformData = OrientationData.orientationData.portraitData;
            RectTransform.anchoredPosition = rectTransformData.anchoredPosition;
            RectTransform.sizeDelta = rectTransformData.sizeDelta;
            RectTransform.pivot = rectTransformData.pivot;
            RectTransform.anchorMin = rectTransformData.anchorMin;
            RectTransform.anchorMax = rectTransformData.anchorMax; 
            RectTransform.localScale = rectTransformData.localScale;
            RectTransform.rotation = rectTransformData.rotation;
            RectTransform.offsetMax = new Vector2(rectTransformData.rightOffsetMaxX, rectTransformData.topOffsetMaxY);
            RectTransform.offsetMin = new Vector2(rectTransformData.leftOffsetMinX, rectTransformData.bottomOffsetMinY);
           

        }

    }
    [ContextMenu("LandscapeLeft")]

    public void ApplyLandscapeLeftOrientation()
    {

        if (OrientationData != null && this.gameObject.activeInHierarchy && orientationUIType == OrientationUIType.Default)
        {
            RectTransformData rectTransformData = OrientationData.orientationData.landscapeLeftData;
            RectTransform.anchoredPosition = rectTransformData.anchoredPosition;
            RectTransform.sizeDelta = rectTransformData.sizeDelta;
            RectTransform.pivot = rectTransformData.pivot;
            RectTransform.anchorMin = rectTransformData.anchorMin;
            RectTransform.anchorMax = rectTransformData.anchorMax;
            RectTransform.localScale = rectTransformData.localScale;
            RectTransform.rotation = rectTransformData.rotation;
            RectTransform.offsetMax = new Vector2(rectTransformData.rightOffsetMaxX, rectTransformData.topOffsetMaxY);
            RectTransform.offsetMin = new Vector2(rectTransformData.leftOffsetMinX, rectTransformData.bottomOffsetMinY);
        }

    }
    public void RevertToOriginal()
    {
        if (OrientationData != null && this.gameObject.activeInHierarchy && orientationUIType == OrientationUIType.Default)
        {
            RectTransformData rectTransformData = OrientationData.orientationData.originalPosition;
            RectTransform.anchoredPosition = rectTransformData.anchoredPosition;
            RectTransform.sizeDelta = rectTransformData.sizeDelta;
            RectTransform.pivot = rectTransformData.pivot;
            RectTransform.anchorMin = rectTransformData.anchorMin;
            RectTransform.anchorMax = rectTransformData.anchorMax;
            RectTransform.localScale = rectTransformData.localScale;
            RectTransform.rotation = rectTransformData.rotation;
            RectTransform.offsetMax = new Vector2(rectTransformData.rightOffsetMaxX, rectTransformData.topOffsetMaxY);
            RectTransform.offsetMin = new Vector2(rectTransformData.leftOffsetMinX, rectTransformData.bottomOffsetMinY);
        }
    }
    [ContextMenu("LandscapeRight")]

    public void ApplyLandscapeRightOrientation()
    {

        if (OrientationData != null && this.gameObject.activeInHierarchy && orientationUIType == OrientationUIType.Default)
        {
            RectTransformData rectTransformData = OrientationData.orientationData.landscapeRightData;
            RectTransform.anchoredPosition = rectTransformData.anchoredPosition;
            RectTransform.sizeDelta = rectTransformData.sizeDelta;
            RectTransform.pivot = rectTransformData.pivot;
            RectTransform.anchorMin = rectTransformData.anchorMin;
            RectTransform.anchorMax = rectTransformData.anchorMax;
            RectTransform.localScale = rectTransformData.localScale;
            RectTransform.rotation = rectTransformData.rotation;
            RectTransform.offsetMax = new Vector2(rectTransformData.rightOffsetMaxX, rectTransformData.topOffsetMaxY);
            RectTransform.offsetMin = new Vector2(rectTransformData.leftOffsetMinX, rectTransformData.bottomOffsetMinY);
        }
    }

    public void SavePortraitOrientationData()
    {
        
        if (OrientationData != null)
        {
            OrientationData.orientationData.portraitData = new RectTransformData
            {
                anchoredPosition = RectTransform.anchoredPosition,
                sizeDelta = RectTransform.sizeDelta,
                rotation = RectTransform.rotation,
                pivot = RectTransform.pivot,
                anchorMin = RectTransform.anchorMin,
                anchorMax = RectTransform.anchorMax,
                localScale = RectTransform.localScale,
                topOffsetMaxY = RectTransform.offsetMax.y,
                bottomOffsetMinY = RectTransform.offsetMin.y,
                rightOffsetMaxX = RectTransform.offsetMax.x,
                leftOffsetMinX = RectTransform.offsetMin.x
                

            }; 
        }
    }
    public void SaveLandscapeLeftOrientationData()
    {
        if (OrientationData != null)
        {
            OrientationData.orientationData.landscapeLeftData = new RectTransformData
            {
                anchoredPosition = RectTransform.anchoredPosition,
                sizeDelta = RectTransform.sizeDelta,
                rotation = RectTransform.rotation,
                pivot = RectTransform.pivot,
                anchorMin = RectTransform.anchorMin,
                anchorMax = RectTransform.anchorMax,
                localScale = RectTransform.localScale,
                topOffsetMaxY = RectTransform.offsetMax.y,
                bottomOffsetMinY = RectTransform.offsetMin.y,
                rightOffsetMaxX = RectTransform.offsetMax.x,
                leftOffsetMinX = RectTransform.offsetMin.x
            }; 
        }
    }
    public void SaveLandscapeRightOrientationData()
    {
        if (OrientationData != null)
        {
            OrientationData.orientationData.landscapeRightData = new RectTransformData
            {
                anchoredPosition = RectTransform.anchoredPosition,
                sizeDelta = RectTransform.sizeDelta,
                rotation = RectTransform.rotation,
                pivot = RectTransform.pivot,
                anchorMin = RectTransform.anchorMin,
                anchorMax = RectTransform.anchorMax,
                localScale = RectTransform.localScale,
                topOffsetMaxY = RectTransform.offsetMax.y,
                bottomOffsetMinY = RectTransform.offsetMin.y,
                rightOffsetMaxX = RectTransform.offsetMax.x,
                leftOffsetMinX = RectTransform.offsetMin.x

            }; 
        }
    }
}
