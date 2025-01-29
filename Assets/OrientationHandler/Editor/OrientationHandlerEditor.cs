using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Security.AccessControl;
using UnityEngine.UIElements;
using System.Linq;
using Unity.VisualScripting;
using TMPro;


namespace OrientationHandlerMain
{

    [CustomEditor(typeof(GameObject))]
    public class OrientationHandlerEditor : Editor
    {
        public static string oritantationDataFilePath;
        public static OrientationHandlerEditor Instance;

        public OrientationHandlerEditor()
        {
            Instance = this;

            oritantationDataFilePath = Application.dataPath + "/OrientationData.json";

            Debug.Log("UI Object Inspector Initialized");

        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUILayout.Space(30);
            GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(1));
            GameObject gameObject = (GameObject)target;
            OrientationUI orientationUI = gameObject.GetComponent<OrientationUI>();
            if (orientationUI != null)
            {
                GUILayout.Space(10);
                GUILayout.Label("Orientation Handler");
                GUILayout.Space(20);

                if (orientationUI.orientationData != null)
                {

                    //EditorGUILayout.Vector3Field("Portrait Position", orientationUI.orientationData.orientationData.portraitData.anchoredPosition); 
                }

                if (GUILayout.Button("Save Portrait Data"))
                {
                    Debug.Log("UI GameObject Name: " + gameObject.name);
                    if (orientationUI.orientationData == null)
                    {
                        ScreenOrientationData screenData = OrientationDataRecorder.CreateScriptableObjectAsset(orientationUI);
                        orientationUI.orientationData = screenData;
                    }


                    orientationUI.SavePortraitOrientationData();
                }

                if (orientationUI.orientationData != null)
                {

                    //EditorGUILayout.Vector3Field("LandscapeLeft Position", orientationUI.orientationData.orientationData.landscapeLeftData.anchoredPosition);
                }

                if (GUILayout.Button("Save Landscape Left Data"))
                {
                    if (orientationUI.orientationData == null)
                    {
                        ScreenOrientationData screenData = OrientationDataRecorder.CreateScriptableObjectAsset(orientationUI);
                        orientationUI.orientationData = screenData;
                    }
                    orientationUI.SaveLandscapeLeftOrientationData();

                }

                if (orientationUI.orientationData != null)
                {
                    if (orientationUI.orientationData == null)
                    {
                        ScreenOrientationData screenData = OrientationDataRecorder.CreateScriptableObjectAsset(orientationUI);
                        orientationUI.orientationData = screenData;
                    }
                    //EditorGUILayout.Vector3Field("LandscapeRight Position", orientationUI.orientationData.orientationData.landscapeRightData.anchoredPosition);
                }

                if (GUILayout.Button("Save Landscape Right Data"))
                {
                    orientationUI.SaveLandscapeRightOrientationData();


                }

                GUILayout.Space(20);

            }
            else if (gameObject.GetComponent<RectTransform>() != null)
            {
                GUILayout.Space(10);
                GUILayout.Label("Orientation Handler");
                GUILayout.Space(20);
                if (GUILayout.Button("Add Orientation UI"))
                {
                    gameObject.AddComponent<OrientationUI>();
                }
                GUILayout.Space(20);
            }
        }


    }
    public class OrientationDataRecorder
    {
        public static List<string> SONames = new List<string>();
        public static int SOCount;
        //public static List<OrientationUI> orientationUIList = new List<OrientationUI>();
        public static ScreenOrientationData CreateScriptableObjectAsset(OrientationUI orientationUI)
        {
            ScreenOrientationData screenData = null;
            if (SONames.Contains(orientationUI.name))
            {
                SOCount += 1;
                string assetName = $"{orientationUI.name}{SOCount}";
                screenData = ScriptableObject.CreateInstance<ScreenOrientationData>();
                AssetDatabase.CreateAsset(screenData, $"Assets/OrientationHandler/ScriptableObjects/OrientationData/{assetName}.asset");
                SONames.Add(assetName);

            }
            else
            {
                screenData = ScriptableObject.CreateInstance<ScreenOrientationData>();
                AssetDatabase.CreateAsset(screenData, $"Assets/OrientationHandler/ScriptableObjects/OrientationData/{orientationUI.name}.asset");
                SONames.Add(orientationUI.name);

            }

            return screenData;
        }

        [MenuItem("Orientation Handler/Revert/All")]
        public static void RevertAllToOriginal()
        {
            Debug.Log("Function Called ResetToDefault");
            OrientationUI[] allObjects = GameObject.FindObjectsOfType<OrientationUI>(true);

            foreach (OrientationUI obj in allObjects)
            {
                obj.RevertToOriginal();
            }
        }
        [MenuItem("Orientation Handler/Revert/Selected")]

        public static void RevertSelectedToOriginal()
        {
            Debug.Log("Function Called ResetToDefault");
            GameObject[] allObjects = Selection.gameObjects;


            foreach (GameObject obj in allObjects)
            {
                if (obj.TryGetComponent<OrientationUI>(out OrientationUI orientationUI))
                {
                    orientationUI.RevertToOriginal();

                }
            }
        }

        [MenuItem("Orientation Handler/Save/All/Portrait")]
        public static void SaveAllPortraitOrientations()
        {

            GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>(true);

            foreach (GameObject obj in allObjects)
            {
                OrientationUI orientationUI = obj.GetComponent<OrientationUI>();

                if (orientationUI != null && obj.GetComponent<Canvas>() == null)
                {
                    if (orientationUI.orientationData != null)
                    {
                        RectTransformData portraitData = GetRectData(orientationUI.RectTransform);
                        orientationUI.OrientationData.orientationData.portraitData = portraitData;
                    }
                    else
                    {

                    }
                }
            }
        }
        [MenuItem("Orientation Handler/Save/Selected/Portrait")]
        public static void SaveSelectedPortraitOrientations()
        {

            GameObject[] allObjects = Selection.gameObjects;

            foreach (GameObject obj in allObjects)
            {
                OrientationUI orientationUI = obj.GetComponent<OrientationUI>();

                if (orientationUI != null && obj.GetComponent<Canvas>() == null)
                {
                    RectTransformData portraitData = GetRectData(orientationUI.RectTransform);
                    orientationUI.OrientationData.orientationData.portraitData = portraitData;
                }
                else
                {
                    Debug.Log("Orientation UI is null or canvas is not null");
                }
            }
        }

        [MenuItem("Orientation Handler/Save/Selected/LandscapeLeft")]
        public static void SaveSelectedLandscapeLeftOrientations()
        {

            GameObject[] allObjects = Selection.gameObjects;

            foreach (GameObject obj in allObjects)
            {
                OrientationUI orientationUI = obj.GetComponent<OrientationUI>();

                if (orientationUI != null && obj.GetComponent<Canvas>() == null)
                {
                    RectTransformData currentData = GetRectData(orientationUI.RectTransform);
                    orientationUI.OrientationData.orientationData.landscapeLeftData = currentData;
                }
            }
        }

        [MenuItem("Orientation Handler/Save/Selected/LandscapeRight")]
        public static void SaveSelectedLandscapeRightOrientations()
        {

            GameObject[] allObjects = Selection.gameObjects;

            foreach (GameObject obj in allObjects)
            {
                OrientationUI orientationUI = obj.GetComponent<OrientationUI>();

                if (orientationUI != null && obj.GetComponent<Canvas>() == null)
                {
                    RectTransformData currentData = GetRectData(orientationUI.RectTransform);
                    orientationUI.OrientationData.orientationData.landscapeRightData = currentData;
                }
            }
        }


        [MenuItem("Orientation Handler/Save/All/LandscapeLeft")]
        public static void SaveAllLandscapeLeftOrientations()
        {
            GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>(true);

            foreach (GameObject obj in allObjects)
            {
                OrientationUI orientationUI = obj.GetComponent<OrientationUI>();

                if (orientationUI != null && obj.GetComponent<Canvas>() == null)
                {
                    try
                    {
                        RectTransformData landScapeData = GetRectData(orientationUI.RectTransform);
                        orientationUI.OrientationData.orientationData.landscapeLeftData = landScapeData;
                    }
                    catch (System.Exception e)
                    {

                        Debug.Log($"{obj.name} {obj.GetInstanceID()}Error object null");
                        obj.name = $"{obj.name} Debug";
                    }
                }
            }
        }

        [MenuItem("Orientation Handler/Save/All/LandscapeRight")]
        public static void SaveAllLandscapeRightOrientations()
        {
            GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>(true);

            foreach (GameObject obj in allObjects)
            {
                OrientationUI orientationUI = obj.GetComponent<OrientationUI>();

                if (orientationUI != null && obj.GetComponent<Canvas>() == null)
                {
                    try
                    {
                        RectTransformData landScapeData = GetRectData(orientationUI.RectTransform);
                        orientationUI.OrientationData.orientationData.landscapeRightData = landScapeData;
                        EditorUtility.SetDirty(orientationUI.orientationData);
                        
                    }
                    catch (System.Exception e)
                    {
                        obj.name = $"{obj.name} - DebugX";
                        Debug.LogError($"{obj.name}: Landscape Right Saving errroe -- {e}");
                        
                    }
                }

            }
            AssetDatabase.SaveAssets();
        }



        [MenuItem("Orientation Handler/Delete All SO Assets")]
        public static void DeleteAllOrientationDataSO()
        {
            SONames.Clear();
            SOCount = 0;
            DeleteAssetsInFolder("Assets/OrientationHandler/ScriptableObjects/OrientationData/", false, false);
        }
        [MenuItem("Orientation Handler/Remove Configurations")]
        public static void RemoveConfigurations()
        {
            SONames.Clear();
            SOCount = 0;
            Debug.Log("This is not implemented yuet");
        }

        [MenuItem("Orientation Handler/Apply/All/Portrait")]
        public static void ResetToPortraitOrientation()
        {
            Debug.Log("Function Called ResetToDefault");
            OrientationUI[] allObjects = GameObject.FindObjectsOfType<OrientationUI>(true);
            //orientationUIList.Clear();

            foreach (OrientationUI obj in allObjects)
            {
                Debug.Log(obj.name);
                obj.ApplyPortraitOrientation();
            }
        }
        [MenuItem("Orientation Handler/Apply/All/LandscapeLeft")]
        public static void ResetToLandscapeLeftOrientation()
        {
            Debug.Log("Function Called ResetToDefault");
            OrientationUI[] allObjects = GameObject.FindObjectsOfType<OrientationUI>(true);
            //orientationUIList.Clear();

            foreach (OrientationUI obj in allObjects)
            {
                obj.ApplyLandscapeLeftOrientation();
            }
        }

        [MenuItem("Orientation Handler/Apply/All/LandscapeRight")]
        public static void ResetToLandscapeRightOrientation()
        {
            Debug.Log("Function Called ResetToDefault");
            OrientationUI[] allObjects = GameObject.FindObjectsOfType<OrientationUI>(true);

            foreach (OrientationUI obj in allObjects)
            {

                obj.ApplyLandscapeRightOrientation();

            }
        }

        [MenuItem("Orientation Handler/Apply/Selected/LandscapeRight")]
        public static void ResetSelectedToLandscapeRightOrientation()
        {
            Debug.Log("Function Called ResetToDefault");
            GameObject[] allObjects = Selection.gameObjects;

            foreach (GameObject obj in allObjects)
            {

                if (obj.TryGetComponent<OrientationUI>(out OrientationUI orientationUI))
                {
                    orientationUI.ApplyLandscapeRightOrientation();
                }

            }
        }
        [MenuItem("Orientation Handler/Apply/Selected/LandscapeLeft")]
        public static void ResetSelectedToLandscapeLeftOrientation()
        {
            Debug.Log("Function Called ResetToDefault");
            GameObject[] allObjects = Selection.gameObjects;

            foreach (GameObject obj in allObjects)
            {

                if (obj.TryGetComponent<OrientationUI>(out OrientationUI orientationUI))
                {
                    orientationUI.ApplyLandscapeLeftOrientation();
                }

            }
        }
        [MenuItem("Orientation Handler/Apply/Selected/Portrait")]
        public static void ResetSelectedToPortraitOrientation()
        {
            Debug.Log("Function Called ResetToDefault");
            GameObject[] allObjects = Selection.gameObjects;

            foreach (GameObject obj in allObjects)
            {

                if (obj.TryGetComponent<OrientationUI>(out OrientationUI orientationUI))
                {
                    orientationUI.ApplyPortraitOrientation();
                }

            }
        }

        private static void DeleteAssetsInFolder(string folderPath, bool includeSubfolders, bool showConfirmation)
        {
            if (!AssetDatabase.IsValidFolder(folderPath))
            {
                EditorUtility.DisplayDialog("Error",
                    $"Invalid folder path: {folderPath}\nPlease ensure the path starts with 'Assets/'",
                    "OK");
                return;
            }

            string fullPath = Path.Combine(Application.dataPath, folderPath.Substring(7));
            if (!Directory.Exists(fullPath))
            {
                EditorUtility.DisplayDialog("Error",
                    $"Folder not found: {folderPath}",
                    "OK");
                return;
            }

            string[] assetGUIDs;
            if (includeSubfolders)
            {
                assetGUIDs = AssetDatabase.FindAssets("", new[] { folderPath });
            }
            else
            {
                // Only get files in the specified folder, not subfolders
                string[] filePaths = Directory.GetFiles(fullPath)
                    .Where(f => !f.EndsWith(".meta"))
                    .Select(f => folderPath + f.Substring(fullPath.Length).Replace('\\', '/'))
                    .ToArray();
                assetGUIDs = filePaths.Select(f => AssetDatabase.AssetPathToGUID(f)).ToArray();
            }

            if (assetGUIDs.Length == 0)
            {
                EditorUtility.DisplayDialog("Info",
                    "No assets found in the specified folder.",
                    "OK");
                return;
            }

            if (showConfirmation)
            {
                bool proceed = EditorUtility.DisplayDialog("Confirm Delete",
                    $"Are you sure you want to delete {assetGUIDs.Length} assets in {folderPath}?",
                    "Yes", "Cancel");

                if (!proceed) return;
            }

            foreach (string guid in assetGUIDs)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                AssetDatabase.DeleteAsset(assetPath);
            }

            AssetDatabase.Refresh();
            EditorUtility.DisplayDialog("Success",
                $"Deleted {assetGUIDs.Length} assets from {folderPath}",
                "OK");
        }
        [MenuItem("Orientation Handler/Create Manager Instance")]
        public static void CreateOrientationHandlingManagerInstance()
        {
            OrientationHandlingManager existingObject = GameObject.FindObjectOfType<OrientationHandlingManager>();
            string prefabPath = "Assets/OrientationHandler/Prefabs/OrientationHandlingManager.prefab";
            if (existingObject != null)
            {
                Debug.LogWarning($"A GameObject with the name already exists in the hierarchy.");
                return;
            }
            if (existingObject == null)
            {
                // Load the prefab asset
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);

                if (prefab == null)
                {
                    Debug.LogError($"Could not find prefab at path: {prefabPath}");
                    return;
                }

                // Instantiate the prefab in the scene
                GameObject instance = PrefabUtility.InstantiatePrefab(prefab) as GameObject;

                if (instance == null)
                {
                    Debug.LogError("Failed to instantiate prefab");
                    return;
                }

                // Register the creation for undo
                Undo.RegisterCreatedObjectUndo(instance, "Instantiate Prefab");


            }
            else
            {
                Debug.Log($"Prefab instance  already exists in the scene");
            }
        }



        //[MenuItem("Orientation Handler/Configure All")]
        public static void ConfigureAll()
        {
            CreateOrientationHandlingManagerInstance();

            GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>(true);
            //orientationUIList.Clear();

            foreach (GameObject obj in allObjects)
            {
                RectTransform rectTransform = obj.GetComponent<RectTransform>();
                TextMeshProUGUI tmp = obj.GetComponent<TextMeshProUGUI>();

                if ((rectTransform != null || tmp != null) && obj.GetComponent<Canvas>() == null)
                {
                    OrientationUI orientationUI = obj.GetComponent<OrientationUI>();

                    if (orientationUI == null)
                    {
                        orientationUI = obj.AddComponent<OrientationUI>();
                    }
                    if (orientationUI.OrientationData == null)
                    {
                        ScreenOrientationData screenData = CreateScriptableObjectAsset(orientationUI);
                        EditorUtility.SetDirty(screenData);
                        orientationUI.OrientationData = screenData;
                        orientationUI.orientationData.orientationData.originalPosition = GetRectData(rectTransform);

                    }
                    //orientationUIList.Add(orientationUI);
                    if (rectTransform != null)
                    {
                        orientationUI.RectTransform = obj.GetComponent<RectTransform>();
                    }

                }
            }
        }


        [MenuItem("Orientation Handler/Configure Selected")]
        public static void ConfigureSelected()
        {
            CreateOrientationHandlingManagerInstance();

            GameObject[] allObjects = Selection.gameObjects;

            foreach (GameObject obj in allObjects)
            {

                RectTransform rectTransform = obj.GetComponent<RectTransform>();
                TextMeshProUGUI tmp = obj.GetComponent<TextMeshProUGUI>();

                if ((rectTransform != null || tmp != null) && obj.GetComponent<Canvas>() == null)
                {
                    OrientationUI orientationUI = obj.GetComponent<OrientationUI>();

                    if (orientationUI == null)
                    {
                        orientationUI = obj.AddComponent<OrientationUI>();
                    }
                    if (orientationUI.OrientationData == null)
                    {
                        ScreenOrientationData screenData = CreateScriptableObjectAsset(orientationUI);
                        orientationUI.OrientationData = screenData;
                        orientationUI.orientationData.orientationData.originalPosition = GetRectData(rectTransform);
                        EditorUtility.SetDirty(screenData);
                        AssetDatabase.SaveAssetIfDirty(orientationUI);


                    }
                    if (rectTransform != null)
                    {
                        orientationUI.RectTransform = obj.GetComponent<RectTransform>();
                    }

                }
            }
        }

        public static RectTransformData GetRectData(RectTransform rect)
        {
            return new RectTransformData()
            {
                anchoredPosition = rect.anchoredPosition,
                sizeDelta = rect.sizeDelta,
                anchorMax = rect.anchorMax,
                anchorMin = rect.anchorMin,
                rotation = rect.rotation,
                pivot = rect.pivot,
                localScale = rect.localScale,
                leftOffsetMinX = rect.offsetMin.x,
                rightOffsetMaxX = rect.offsetMax.x,
                topOffsetMaxY = rect.offsetMax.y,
                bottomOffsetMinY = rect.offsetMin.y,
            };
        }

    }



}


