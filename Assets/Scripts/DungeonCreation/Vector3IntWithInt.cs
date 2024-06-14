using UnityEngine;

[System.Serializable]
public struct Vector3IntWithInt
{
    public Vector3 position;
    public int value;

    public Vector3IntWithInt(Vector3 position, int value)
    {
        this.position = position;
        this.value = value;
    }
}


[System.Serializable]
public struct DungeonGenBaseObject
{
    // public (enum)Type Terrain?
    public GameObject ObjectPrefab;
    public Transform ObjectPos;
    //public
}
