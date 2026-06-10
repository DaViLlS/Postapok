using System.Collections.Generic;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;

namespace _Project.Editor
{
    [EditorTool("Prefab Brush 2D")]
    public class PrefabBrush2D : EditorTool
    {
        private GUIContent icon;
    
        // Категории и префабы
        private List<CategoryData> categories = new List<CategoryData>();
        private int selectedCategoryIndex = -1;
        private int selectedPrefabIndex = -1;
        
        // Настройки кисти
        private float brushSize = 1f;
        private bool randomRotation = false;
        private bool randomScale = false;
        private Vector2 scaleRange = new Vector2(0.8f, 1.2f);
        private Transform paintParent;
        
        private bool isPainting = false;
        private GameObject previewObject;
        
        private const string PREFS_KEY = "PrefabBrush2D_Categories";
        
        [System.Serializable]
        public class CategoryData
        {
            public string name;
            public List<string> prefabGuids = new List<string>();
            [System.NonSerialized]
            public List<GameObject> prefabs = new List<GameObject>();
            
            public void SavePrefabs()
            {
                prefabGuids.Clear();
                foreach (var prefab in prefabs)
                {
                    if (prefab != null)
                    {
                        string guid = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(prefab));
                        if (!string.IsNullOrEmpty(guid))
                            prefabGuids.Add(guid);
                    }
                }
            }
            
            public void LoadPrefabs()
            {
                prefabs.Clear();
                foreach (string guid in prefabGuids)
                {
                    string path = AssetDatabase.GUIDToAssetPath(guid);
                    if (!string.IsNullOrEmpty(path))
                    {
                        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                        if (prefab != null)
                            prefabs.Add(prefab);
                    }
                }
            }
        }
        
        [System.Serializable]
        private class SerializationWrapper
        {
            public List<CategoryData> categories;
        }
        
        void OnEnable()
        {
            icon = EditorGUIUtility.TrIconContent("d_Prefab Icon", "Prefab Brush 2D");
            SceneView.duringSceneGui += OnSceneGUI;
            LoadData();
        }
        
        void OnDisable()
        {
            SceneView.duringSceneGui -= OnSceneGUI;
            DestroyPreview();
        }
        
        void DestroyPreview()
        {
            if (previewObject != null)
            {
                DestroyImmediate(previewObject);
                previewObject = null;
            }
        }
        
        public override GUIContent toolbarIcon => icon;
        
        GameObject GetSelectedPrefab()
        {
            if (selectedCategoryIndex < 0 || selectedCategoryIndex >= categories.Count) return null;
            if (categories[selectedCategoryIndex].prefabs == null) return null;
            if (selectedPrefabIndex < 0 || selectedPrefabIndex >= categories[selectedCategoryIndex].prefabs.Count) return null;
            return categories[selectedCategoryIndex].prefabs[selectedPrefabIndex];
        }
        
        // Окно настроек
        public override void OnActivated()
        {
            PrefabBrushWindow.ShowWindow(this);
        }
        
        public Vector3 GetMouseWorldPosition()
        {
            Event e = Event.current;
            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            
            // 2D рейкаст
            RaycastHit2D hit2D = Physics2D.GetRayIntersection(ray, 1000f);
            if (hit2D.collider != null)
                return hit2D.point;
            
            // Плоскость XY
            Plane plane = new Plane(Vector3.back, Vector3.zero);
            if (plane.Raycast(ray, out float dist))
                return ray.GetPoint(dist);
            
            return ray.GetPoint(10f);
        }
        
        void OnSceneGUI(SceneView sceneView)
        {
            Event e = Event.current;
            
            if (e.type == EventType.Layout || e.type == EventType.Repaint)
                return;
            
            Vector3 point = GetMouseWorldPosition();
            var selectedPrefab = GetSelectedPrefab();
            
            // Превью
            if (selectedPrefab != null && previewObject == null)
            {
                previewObject = Instantiate(selectedPrefab);
                previewObject.hideFlags = HideFlags.HideAndDontSave;
                previewObject.name = "PREVIEW";
                
                // Полупрозрачность
                var renderers = previewObject.GetComponentsInChildren<SpriteRenderer>();
                foreach (var r in renderers)
                {
                    Color c = r.color;
                    c.a = 0.5f;
                    r.color = c;
                }
            }
            
            if (previewObject != null)
            {
                previewObject.transform.position = point;
                
                if (randomRotation)
                    previewObject.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
            }
            
            // Рисуем гизмо
            Handles.color = new Color(0, 1, 0, 0.3f);
            Handles.DrawSolidDisc(point, Vector3.forward, brushSize);
            Handles.color = Color.green;
            Handles.DrawWireDisc(point, Vector3.forward, brushSize);
            
            // Клик
            if (e.type == EventType.MouseDown && e.button == 0 && !e.alt)
            {
                if (selectedPrefab != null)
                {
                    PlacePrefab(point);
                    e.Use();
                }
            }
            
            // Drag для кисти
            if (e.type == EventType.MouseDrag && e.button == 0 && !e.alt)
            {
                if (selectedPrefab != null)
                {
                    PlacePrefab(point);
                    e.Use();
                }
            }
            
            sceneView.Repaint();
        }
        
        void PlacePrefab(Vector3 position)
        {
            var prefab = GetSelectedPrefab();
            if (prefab == null) return;
            
            GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            instance.transform.position = position;
            
            if (randomRotation)
                instance.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
            
            if (randomScale)
            {
                float scale = Random.Range(scaleRange.x, scaleRange.y);
                instance.transform.localScale = Vector3.one * scale;
            }
            
            if (paintParent != null)
                instance.transform.SetParent(paintParent);
            
            Undo.RegisterCreatedObjectUndo(instance, "Paint Prefab 2D");
            Selection.activeGameObject = instance;
        }
        
        void SaveData()
        {
            foreach (var cat in categories)
                cat.SavePrefabs();
            
            string json = JsonUtility.ToJson(new SerializationWrapper { categories = this.categories });
            EditorPrefs.SetString(PREFS_KEY, json);
        }
        
        void LoadData()
        {
            if (EditorPrefs.HasKey(PREFS_KEY))
            {
                string json = EditorPrefs.GetString(PREFS_KEY);
                var wrapper = JsonUtility.FromJson<SerializationWrapper>(json);
                if (wrapper != null && wrapper.categories != null)
                {
                    categories = wrapper.categories;
                    foreach (var cat in categories)
                        cat.LoadPrefabs();
                }
            }
            
            if (categories.Count == 0)
            {
                categories.Add(new CategoryData { name = "🌳 Деревья" });
                categories.Add(new CategoryData { name = "🌿 Кусты" });
                categories.Add(new CategoryData { name = "🏠 Дома" });
                categories.Add(new CategoryData { name = "🚧 Заборы" });
                SaveData();
            }
        }
        
        // Окно для выбора префабов
        public class PrefabBrushWindow : EditorWindow
        {
            private PrefabBrush2D brush;
            private Vector2 categoriesScroll;
            private Vector2 prefabsScroll;
            
            public static void ShowWindow(PrefabBrush2D brushTool)
            {
                var window = GetWindow<PrefabBrushWindow>("Prefab Brush 2D");
                window.brush = brushTool;
                window.minSize = new Vector2(300, 400);
            }
            
            void OnGUI()
            {
                if (brush == null) return;
                
                // Настройки
                brush.brushSize = EditorGUILayout.Slider("Размер кисти", brush.brushSize, 0.5f, 5f);
                brush.randomRotation = EditorGUILayout.Toggle("Случайный поворот", brush.randomRotation);
                brush.randomScale = EditorGUILayout.Toggle("Случайный масштаб", brush.randomScale);
                if (brush.randomScale)
                    brush.scaleRange = EditorGUILayout.Vector2Field("Диапазон", brush.scaleRange);
                
                brush.paintParent = (Transform)EditorGUILayout.ObjectField("Родитель", brush.paintParent, typeof(Transform), true);
                
                EditorGUILayout.Space(10);
                
                // Категории
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("+ Категория"))
                {
                    brush.categories.Add(new CategoryData { name = "Новая" });
                    brush.selectedCategoryIndex = brush.categories.Count - 1;
                    brush.SaveData();
                }
                if (GUILayout.Button("-") && brush.selectedCategoryIndex >= 0)
                {
                    brush.categories.RemoveAt(brush.selectedCategoryIndex);
                    brush.selectedCategoryIndex = -1;
                    brush.selectedPrefabIndex = -1;
                    brush.DestroyPreview();
                    brush.SaveData();
                }
                EditorGUILayout.EndHorizontal();
                
                EditorGUILayout.BeginHorizontal();
                
                // Список категорий
                EditorGUILayout.BeginVertical(GUILayout.Width(100));
                categoriesScroll = EditorGUILayout.BeginScrollView(categoriesScroll);
                for (int i = 0; i < brush.categories.Count; i++)
                {
                    GUI.backgroundColor = (i == brush.selectedCategoryIndex) ? Color.cyan : Color.white;
                    if (GUILayout.Button(brush.categories[i].name, GUILayout.Height(25)))
                    {
                        brush.selectedCategoryIndex = i;
                        brush.selectedPrefabIndex = -1;
                        brush.DestroyPreview();
                    }
                }
                GUI.backgroundColor = Color.white;
                EditorGUILayout.EndScrollView();
                EditorGUILayout.EndVertical();
                
                // Префабы
                EditorGUILayout.BeginVertical();
                if (brush.selectedCategoryIndex >= 0)
                {
                    var cat = brush.categories[brush.selectedCategoryIndex];
                    cat.name = EditorGUILayout.TextField(cat.name);
                    
                    if (GUILayout.Button("+ Добавить из Selection"))
                    {
                        var selected = Selection.gameObjects;
                        foreach (var go in selected)
                        {
                            var src = PrefabUtility.GetCorrespondingObjectFromSource(go) as GameObject;
                            if (src == null) src = go;
                            if (AssetDatabase.Contains(src) && !cat.prefabs.Contains(src))
                                cat.prefabs.Add(src);
                        }
                        brush.SaveData();
                    }
                    
                    prefabsScroll = EditorGUILayout.BeginScrollView(prefabsScroll);
                    for (int i = 0; i < cat.prefabs.Count; i++)
                    {
                        GUI.backgroundColor = (i == brush.selectedPrefabIndex) ? Color.green : Color.white;
                        if (cat.prefabs[i] != null)
                        {
                            if (GUILayout.Button(cat.prefabs[i].name, GUILayout.Height(30)))
                            {
                                brush.selectedPrefabIndex = i;
                                brush.DestroyPreview();
                            }
                        }
                    }
                    GUI.backgroundColor = Color.white;
                    EditorGUILayout.EndScrollView();
                }
                EditorGUILayout.EndVertical();
                
                EditorGUILayout.EndHorizontal();
            }
        }
    }
}