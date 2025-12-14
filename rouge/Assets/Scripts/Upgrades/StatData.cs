using UnityEngine;

[CreateAssetMenu(fileName = "NewStat", menuName = "Wave System/Stat Data")]
public class StatData : ScriptableObject
{
    public GameObject[] segmentTypes;
    public int level = 1;
    public float maxCapacity = 1;
}