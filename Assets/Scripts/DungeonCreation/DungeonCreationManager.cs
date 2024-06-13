using System.Collections.Generic;
using UnityEngine;

public class DungeonCreationManager : MonoBehaviour
{
    public static DungeonCreationManager Instance { get; private set; }

    [System.Serializable]
    private struct SpawnableObject
    {
        public string name;
        public GameObject prefab;
    }

    [SerializeField] private List<Vector3IntWithInt> placeTerrainKV = new(); // TODO: should save structs/classes which hold more data than 
    [SerializeField] private SavedMap saved;
    [SerializeField] private List<SpawnableObject> terrainTypes;
    [SerializeField] private List<SpawnableObject> objectTypes;
    [SerializeField] private List<SpawnableObject> itemTypes;

    public GameObject selectedPrefab;

    public Mode currentMode;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public List<string> GetNamesForCurrentMode()
    {
        switch (currentMode)
        {
            case Mode.Terrain:
                return GetNames(terrainTypes);
            case Mode.Object:
                return GetNames(objectTypes);
            case Mode.Item:
                return GetNames(itemTypes);
            default:
                return new List<string>();
        }
    }

    public GameObject GetPrefabByName(string name)
    {
        switch (currentMode)
        {
            case Mode.Terrain:
                return GetPrefabByName(terrainTypes, name);
            case Mode.Object:
                return GetPrefabByName(objectTypes, name);
            case Mode.Item:
                return GetPrefabByName(itemTypes, name);
            default:
                return null;
        }
    }

    public void SetSelectedPrefab(string name)
    {
        selectedPrefab = GetPrefabByName(name);
    }

    //public void InstantiateObject()
    //{
    //    Vector3 spawnVector = previewVectorCache + new Vector3(0, 2, 0);
    //    Instantiate(TESTETSE, SetTerrainTransform(spawnVector), Quaternion.identity);

    //    creationManager.placeTerrainKV.Add(new Vector3IntWithInt(SetTerrainTransform(spawnVector), 1));
    //}

    private List<string> GetNames(List<SpawnableObject> objects)
    {
        List<string> names = new List<string>();
        foreach (var obj in objects)
        {
            names.Add(obj.name);
        }
        return names;
    }

    private GameObject GetPrefabByName(List<SpawnableObject> objects, string name)
    {
        foreach (var obj in objects)
        {
            if (obj.name == name)
            {
                return obj.prefab;
            }
        }
        return null;
    }

    public void BtnSaveMap() // make a scriptable object with the data
    {
        SavedMap savedMap = ScriptableObject.CreateInstance<SavedMap>();
        savedMap.placeTerrainKV = placeTerrainKV;

        string assetPath = "Assets/SavedMaps/NewSavedMap.asset";
        UnityEditor.AssetDatabase.CreateAsset(savedMap, assetPath);
        UnityEditor.AssetDatabase.SaveAssets();
    }

    public void BtnLoadMap()
    {
        foreach (Vector3IntWithInt kv in saved.placeTerrainKV)
        {
            Debug.LogWarning("spawning ");
            Instantiate(terrainTypes[0].prefab, kv.position, Quaternion.identity);
        }
    }
}

public enum Mode
{
    Terrain,
    Object,
    Item,
}
