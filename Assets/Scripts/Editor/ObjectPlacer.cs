#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class ObjectPlacer : EditorWindow
{
    private class PrefabElement
    {
        public Object prefab;
        public Texture2D preview;
        public bool active;

        public PrefabElement(Object prefab)
        {
            this.prefab = prefab;
            RefreshPreview();
            active = true;
        }

        public void RefreshPreview()
        {
            if (!preview || preview == defaultIcon)
            {
                preview = AssetPreview.GetAssetPreview(prefab);
                if (!preview)
                    preview = defaultIcon;
            }
        }
    }

    private delegate bool RayMeshDelegate(Ray ray, Mesh mesh, Matrix4x4 matrix, out RaycastHit hit);

    public LayerMask collideMask;
    public bool placeOnMesh = true;
    public bool alignToSurface = true;
    public bool alignToBounds = true;
    public bool allowRotation = true;
    public bool allowScale = true;
    public bool randomRotation = true;
    public bool attachToSelected = true;
    public Vector3 offset = Vector3.zero;

    private bool visible = true;
    private bool active = false;
    private List<PrefabElement> selectedPrefabs;
    private List<PrefabElement> activePrefabs;

    private GameObject currentPlaced;
    private Bounds currentBounds;
    private float currentRadius;
    private Vector3 currentScale;
    private Vector3 surfaceNormal;
    private Vector3 surfacePoint;
    private float surfaceDistance;

    private Vector2 globalScroll;
    private Vector2 prefabsScroll;

    private static Texture2D defaultIcon;
    private MethodInfo RayMeshMethod;
    private RayMeshDelegate rayMeshDelegate;

    [MenuItem("Tools/Object Placer")]
    private static void Init()
    {
        // Get existing open window or if none, make a new one:
        ObjectPlacer window = (ObjectPlacer)EditorWindow.GetWindow(typeof(ObjectPlacer));
        window.titleContent = new GUIContent {text = "Object Placer"};
        window.Show();
    }

    private void OnBecameInvisible()
    {
        visible = false;
    }

    private void OnBecameVisible()
    {
        visible = true;
    }

    private void Awake()
    {
        defaultIcon = EditorGUIUtility.IconContent("d_Prefab Icon").image as Texture2D;


#if UNITY_2020_1_OR_NEWER
        SceneView.duringSceneGui += OnSceneGUI;
#else
        SceneView.onSceneGUIDelegate += OnSceneGUI;
#endif
    }

    void OnDestroy()
    {
#if UNITY_2020_1_OR_NEWER
        SceneView.duringSceneGui -= OnSceneGUI;
#else
        SceneView.onSceneGUIDelegate -= OnSceneGUI;
#endif
    }

    private void OnDisable()
    {
#if UNITY_2020_1_OR_NEWER
        SceneView.duringSceneGui -= OnSceneGUI;
#else
        SceneView.onSceneGUIDelegate -= OnSceneGUI;
#endif
    }

    private void OnEnable()
    {
        collideMask = LayerMask.GetMask("Default");
        selectedPrefabs = new List<PrefabElement>();
        activePrefabs = new List<PrefabElement>();

        RayMeshMethod = typeof(HandleUtility).GetMethod("IntersectRayMesh",BindingFlags.NonPublic | BindingFlags.Static);
        if (RayMeshMethod != null)
        {
            rayMeshDelegate = (RayMeshDelegate) Delegate.CreateDelegate(typeof(RayMeshDelegate), null, RayMeshMethod);
        }

#if UNITY_2020_1_OR_NEWER
        SceneView.duringSceneGui += OnSceneGUI;
#else
        SceneView.onSceneGUIDelegate += OnSceneGUI;
#endif
    }

    private bool RaycastWorld(Vector2 position, out RaycastHit hit)
    {
        hit = new RaycastHit();
        GameObject picked = HandleUtility.PickGameObject(position, false);
        if (!picked)
            return false;

        bool hasHit = false;

        Ray mouseRay = HandleUtility.GUIPointToWorldRay(position);

        // Loop through all meshes and find the RaycastHit closest to the ray origin.
        MeshFilter[] meshFil = picked.GetComponentsInChildren<MeshFilter>();
        float minT = Mathf.Infinity;
        foreach (MeshFilter mf in meshFil)
        {
            Mesh mesh = mf.sharedMesh;
            if (!mesh)
                continue;
            RaycastHit localHit;
            if (rayMeshDelegate.Invoke(mouseRay, mesh, mf.transform.localToWorldMatrix, out localHit))
            {
                if (localHit.distance < minT)
                {
                    hit = localHit;
                    minT = hit.distance;
                    hasHit = true;
                }
            }
        }

        Debug.Log(hit.normal);

        if (minT == Mathf.Infinity)
        {
            // If we didn't find any surface based on meshes, try with colliders.
            Collider[] colliders = picked.GetComponentsInChildren<Collider>();
            foreach (Collider col in colliders)
            {
                RaycastHit localHit;
                if (col.Raycast(mouseRay, out localHit, Mathf.Infinity))
                {
                    if (localHit.distance < minT)
                    {
                        hit = localHit;
                        minT = hit.distance;
                        hasHit = true;
                    }
                }
            }
        }

        /*if (minT == Mathf.Infinity)
        {
            // If we didn't hit any mesh or collider surface, then use the transform position projected onto the ray.
            hit.point = Vector3.Project(picked.transform.position - mouseRay.origin, mouseRay.direction) + mouseRay.origin;
        }*/
        return hasHit;
    }

    private bool Raycast(out RaycastHit hit)
    {
        hit = new RaycastHit();

        if (placeOnMesh && rayMeshDelegate != null)
        {
            return RaycastWorld(Event.current.mousePosition, out hit);
        }

        Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        return Physics.Raycast(ray, out hit, 1000, collideMask, QueryTriggerInteraction.Ignore);
    }

    private void OnSceneGUI(SceneView obj)
    {
        if (visible && active)
        {
            Event e = Event.current;

            if (e.type == EventType.Layout)
            {
                HandleUtility.AddDefaultControl(0);
            }

            if (e.type == EventType.MouseDown && e.button == 0 && selectedPrefabs.Count > 0 && activePrefabs.Count > 0)
            {
                
                if (Raycast(out RaycastHit hit))
                {
                    Object prefab = activePrefabs[Random.Range(0, activePrefabs.Count)].prefab;

                    if (prefab)
                    {
                        currentPlaced = PrefabUtility.InstantiatePrefab(prefab) as GameObject;

                        currentPlaced.transform.position = Vector3.zero;

                        currentScale = currentPlaced.transform.localScale;

                        surfacePoint = hit.point;

                        if (alignToSurface)
                            surfaceNormal = hit.normal.normalized;
                        else
                            surfaceNormal = Vector3.up;

                        Renderer[] renderers = currentPlaced.GetComponentsInChildren<Renderer>();

                        if (renderers.Length > 0)
                        {
                            currentBounds = renderers[0].bounds;

                            foreach (Renderer renderer in renderers)
                            {
                                currentBounds.Encapsulate(renderer.bounds);
                            }
                            currentRadius = (currentBounds.center + currentBounds.extents).magnitude;
                        }

                        if (alignToBounds)
                            currentPlaced.transform.position = surfacePoint + offset + surfaceNormal * currentBounds.extents.y;
                        else
                            currentPlaced.transform.position = surfacePoint + offset;

                        currentPlaced.transform.rotation = Quaternion.LookRotation(Vector3.forward, surfaceNormal);

                        if(!allowRotation && randomRotation)
                            currentPlaced.transform.Rotate(new Vector3(0,Random.Range(0f,360f),0), Space.Self);

                        if(allowScale)
                            currentPlaced.transform.localScale = Vector3.zero;

                        Undo.RegisterCreatedObjectUndo(currentPlaced, "Create object");
                    }
                }
                e.Use();
            }
            else if (e.type == EventType.MouseUp && e.button == 0 && currentPlaced)
            {
                if (attachToSelected && Selection.activeTransform)
                {
                    currentPlaced.transform.SetParent(Selection.activeTransform, true);
                }
                currentPlaced = null;
                e.Use();
            }


            if (currentPlaced)
            {
                Vector3 mousePos = e.mousePosition;
                float ppp = EditorGUIUtility.pixelsPerPoint;
                mousePos.y = obj.camera.pixelHeight - mousePos.y * ppp;
                mousePos.x *= ppp;

                Ray ray = obj.camera.ScreenPointToRay(mousePos);

                Plane objPlane = new Plane(surfaceNormal, surfacePoint);
                objPlane.Raycast(ray, out float dist);
                Vector3 hit = ray.GetPoint(dist);

                Vector3 dir = hit - surfacePoint;

                Handles.color = Color.cyan;
                Handles.DrawWireDisc(surfacePoint, surfaceNormal, dir.magnitude);
                Handles.DrawLine(surfacePoint, hit);

                if (dir != Vector3.zero)
                {
                    if (allowRotation)
                        currentPlaced.transform.rotation = Quaternion.LookRotation(dir, surfaceNormal);
                    if (allowScale)
                    {
                        float scale = dir.magnitude / currentRadius;

                        currentPlaced.transform.localScale = currentScale * scale;
                        if (alignToBounds)
                        {
                            currentPlaced.transform.position =
                                surfacePoint + offset + surfaceNormal * currentBounds.extents.y * scale;
                        }
                    }
                }
            }
            else
            {
                if (e.type == EventType.MouseMove)
                {
                    if (Raycast(out RaycastHit hit))
                    {
                        surfacePoint = hit.point;
                        surfaceNormal = hit.normal.normalized;
                        surfaceDistance = hit.distance;
                    }
                    else
                    {
                        surfaceDistance = 0;
                    }
                }
                else
                {
                    Handles.color = Color.cyan;
                    Handles.DrawWireDisc(surfacePoint, surfaceNormal, surfaceDistance/30f);
                }
            }
        }
    }

    private void AddPrefabs(Object[] objects)
    {
        foreach (Object obj in objects)
        {
            if (obj is GameObject && AssetDatabase.Contains(obj) && !selectedPrefabs.Exists(p => p.prefab == obj))
            {
                selectedPrefabs.Add(new PrefabElement(obj));
            }
        }

        RefreshActives();
    }

    private void RefreshActives()
    {
        activePrefabs = selectedPrefabs.FindAll(p => p.active).ToList();
        if (selectedPrefabs.Count <= 0)
            active = false;

        foreach (var prefab in selectedPrefabs)
        {
            prefab.RefreshPreview();
        }
    }

    private void OnGUI()
    {
        Event evt = Event.current;

        globalScroll = EditorGUILayout.BeginScrollView(globalScroll);

        GUI.enabled = selectedPrefabs.Count > 0;
        GUI.backgroundColor = active ? Color.red : Color.green;
        if (GUILayout.Button(active ? "Disable Placement" : "Enable Placement"))
            active = !active;
        GUI.backgroundColor = Color.white;
        GUI.enabled = true;

        EditorGUILayout.Space(20);

        if(GUILayout.Button("Fill From Selection"))
            AddPrefabs(Selection.objects);

        GUI.backgroundColor = Color.red;
        if (GUILayout.Button("Clear"))
        {
            selectedPrefabs.Clear();
            activePrefabs.Clear();
            RefreshActives();
        }
        GUI.backgroundColor = Color.white;

        var labelStyle = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter };
        labelStyle.normal.textColor = Color.gray;
        EditorGUILayout.LabelField("Right click to delete, middle click to select only one", labelStyle, GUILayout.ExpandWidth(true));

        EditorGUILayout.Space(5);

        int nbPrefabs = selectedPrefabs.Count;
        float buttonWidth = 50;
        float panelWidth = Screen.width;

        int buttonsPerRow = Mathf.FloorToInt(panelWidth / buttonWidth);
        int rows = Mathf.CeilToInt((float)nbPrefabs / (float)buttonsPerRow);

        Rect prefabArea = GUILayoutUtility.GetRect(0.0f, Mathf.Clamp(rows * buttonWidth, 50, 250), GUILayout.ExpandWidth(true));

        if (nbPrefabs > 0)
        {
            prefabsScroll = GUI.BeginScrollView(prefabArea, prefabsScroll, new Rect(0, 0, prefabArea.width - 20, Mathf.Max(50, rows * buttonWidth)));

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < buttonsPerRow; c++)
                {
                    int i = r * buttonsPerRow + c;
                    if (i < selectedPrefabs.Count)
                    {
                        PrefabElement prefab = selectedPrefabs[i];

                        GUI.color = prefab.active ? Color.white : Color.gray;
                        if (GUI.Button(new Rect(c * buttonWidth, r * buttonWidth, buttonWidth, buttonWidth),
                            prefab.preview))
                        {
                            if (evt.button == 0)
                            {
                                prefab.active = !prefab.active;
                            }
                            else if(evt.button == 1)
                            {
                                selectedPrefabs.Remove(prefab);
                            }else if (evt.button == 2)
                            {
                                foreach (var pref in selectedPrefabs)
                                    pref.active = false;
                                prefab.active = true;
                            }
                            
                            RefreshActives();
                        }
                    }

                }
            }

            GUI.color = Color.white;

            GUI.EndScrollView();
        }
        else
        {
            GUI.Box(prefabArea, "Drop Prefabs here");
        }

        EditorGUILayout.Space(5);

        EditorGUILayout.BeginHorizontal();

        GUI.backgroundColor = Color.green;
        if (GUILayout.Button("All"))
        {
            foreach (var prefab in selectedPrefabs)
                prefab.active = true;
            RefreshActives();
        }
        GUI.backgroundColor = Color.red;
        if (GUILayout.Button("None"))
        {
            foreach (var prefab in selectedPrefabs)
                prefab.active = false;
            RefreshActives();
        }
        GUI.backgroundColor = Color.white;

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space(10);

        placeOnMesh = EditorGUILayout.Toggle("Place On Mesh", placeOnMesh);
        GUI.enabled = !placeOnMesh;
        LayerMask tempMask = EditorGUILayout.MaskField("Placement LayerMask",InternalEditorUtility.LayerMaskToConcatenatedLayersMask(collideMask), InternalEditorUtility.layers);
        collideMask = InternalEditorUtility.ConcatenatedLayersMaskToLayerMask(tempMask);
        GUI.enabled = true;

        attachToSelected = EditorGUILayout.Toggle("Attach To Selected Object", attachToSelected);
        alignToSurface  = EditorGUILayout.Toggle("Align To Surface", alignToSurface);
        alignToBounds = EditorGUILayout.Toggle("Align To Object Bounds", alignToBounds);
        allowRotation = EditorGUILayout.Toggle("Allow Rotation", allowRotation);
        allowScale = EditorGUILayout.Toggle("Allow Scale", allowScale);
        GUI.enabled = !allowRotation;
        randomRotation = EditorGUILayout.Toggle("Random Rotation", randomRotation);
        GUI.enabled = true;
        offset = EditorGUILayout.Vector3Field("Offset", offset);

        EditorGUILayout.EndScrollView();

        GUIStyle creditStyle = new GUIStyle{alignment = TextAnchor.UpperRight};
        creditStyle.normal.textColor = Color.gray;
        GUI.Label(new Rect(0, Screen.height-40,Screen.width-5,70),"Object Placer 1.0 by Ikeiwa", creditStyle);

        //Manage Drag and drop
        switch (evt.type)
        {
            case EventType.DragUpdated:
            case EventType.DragPerform:
                if (!prefabArea.Contains(evt.mousePosition))
                    return;

                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                if (evt.type == EventType.DragPerform)
                {
                    DragAndDrop.AcceptDrag();

                    AddPrefabs(DragAndDrop.objectReferences);
                }
                break;
        }
    }
}
#endif