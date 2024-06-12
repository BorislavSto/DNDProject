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
