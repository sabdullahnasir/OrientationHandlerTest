using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class OrientationHandlingManager : MonoBehaviour
{
    public static OrientationHandlingManager Instance;
    public List<OrientationUI> orientationUIList;
    public bool debug;
    public bool generateListOnRuntime;
    public bool senseOrientation;
    public bool enableSensingOnStart;

    public enum SetOrientation { Portrait, LandscapeLeft, LandscapeRight };
    public SetOrientation setOrientation;


    public UnityEvent OnOrientationChangedToPortrait;
    public UnityEvent OnOrientationChangedToLandscapeLeft;
    public UnityEvent OnOrientationChangedToLandscapeRight;

    private void Awake()
    {
        Instance = this;

        if (generateListOnRuntime)
        {

            if (orientationUIList.Count == 0)
            {
                GetAllOrientationUI();
            }
        }
        if (enableSensingOnStart)
        {
            ApplyPortraitOrientation();
            EnableOrientationSensing();
        }


    }

    private void Update()
    {
        if (senseOrientation)
        {
            switch (Screen.orientation)
            {
                case ScreenOrientation.LandscapeLeft:

                    if (setOrientation != SetOrientation.LandscapeLeft)
                    {
                        ApplyLandscapeLeftOrientation();
                    }
                    break;
                case ScreenOrientation.LandscapeRight:
                    if (setOrientation != SetOrientation.LandscapeRight)
                    {
                        ApplyLandscapeRightOrientation();
                    }
                    break;
                case ScreenOrientation.Portrait:
                    if (setOrientation != SetOrientation.Portrait)
                    {
                        ApplyPortraitOrientation();
                    }
                    break;
                case ScreenOrientation.PortraitUpsideDown:
                    if (setOrientation != SetOrientation.Portrait)
                    {
                        ApplyPortraitOrientation();
                    }
                    break;


            }

        }
    }
    public void GetAllOrientationUI()
    {
        orientationUIList = new List<OrientationUI>(FindObjectsOfType<OrientationUI>());
    }

    private void ApplyOrientationRecursively(System.Action<OrientationUI> applyMethod)
    {
        foreach (var orientation in orientationUIList)
        {
            ApplyOrientationToChildren(orientation.transform, applyMethod);
        }
    }

    private void ApplyOrientationToChildren(Transform parentTransform, System.Action<OrientationUI> applyMethod)
    {
        var orientationUI = parentTransform.GetComponent<OrientationUI>();
        if (orientationUI != null)
        {
            applyMethod(orientationUI);
        }

        // Recursively apply to children
        foreach (Transform child in parentTransform)
        {
            ApplyOrientationToChildren(child, applyMethod);
        }
    }

    [ContextMenu("Portrait")]
    public void ApplyPortraitOrientation()
    {
        Debug.Log("Applying Portrait Orientation");
        ApplyOrientationRecursively((o) => o.ApplyPortraitOrientation());
        setOrientation = SetOrientation.Portrait;
        OnOrientationChangedToPortrait.Invoke();


    }

    [ContextMenu("LandscapeLeft")]
    public void ApplyLandscapeLeftOrientation()
    {
        Debug.Log("Applying Landscape Left Orientation");

        ApplyOrientationRecursively((o) => o.ApplyLandscapeLeftOrientation());
        setOrientation = SetOrientation.LandscapeLeft;
        OnOrientationChangedToLandscapeLeft.Invoke();
    }

    [ContextMenu("LandscapeRight")]
    public void ApplyLandscapeRightOrientation()
    {
        Debug.Log("Applying Landscape Right Orientation");

        ApplyOrientationRecursively((o) => o.ApplyLandscapeRightOrientation());
        setOrientation = SetOrientation.LandscapeRight;
        OnOrientationChangedToLandscapeRight.Invoke();

    }
    [ContextMenu("Enable Orientation")]
    public void EnableOrientationSensing()
    {
        Screen.orientation = ScreenOrientation.AutoRotation;
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;
        Screen.autorotateToPortrait = true;
        Screen.autorotateToPortraitUpsideDown = true;
        senseOrientation = true;

    }
    [ContextMenu("Disable Orientation")]

    public void DisableOrientationSensing()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        Screen.autorotateToLandscapeLeft = false;
        Screen.autorotateToLandscapeRight = false;
        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;

        senseOrientation = false;
    }
}
