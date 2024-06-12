using System.Collections.Generic;
using UnityEngine;

public class DungeonCreationManager : MonoBehaviour
{
    [SerializeField] public List<Vector3IntWithInt> placeTerrainKV = new(); // TODO: should save structs/classes which hold more data than 
    [SerializeField] public SavedMap saved;
    [SerializeField] private GameObject TESTETSE;

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
            Instantiate(TESTETSE, kv.position, Quaternion.identity);
        }
    }
}
