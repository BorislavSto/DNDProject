using UnityEngine;

[CreateAssetMenu(fileName = "DungeonCreationBaseObject", menuName = "ScriptableObjects/DungeonObject")]
public class DungeonGenBaseObject : ScriptableObject
{
    public Mode ObjectType;
    public string ObjectName;
    public GameObject ObjectPrefab;
    public Transform ObjectPos;
}
