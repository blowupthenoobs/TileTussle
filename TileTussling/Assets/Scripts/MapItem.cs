using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MapStuff;

[CreateAssetMenu(menuName = "MapItem")]
public class MapItem : ScriptableObject
{
    [SerializeField]
    public Map map;
    public int treeSpawns;
    public int rockSpawns;
}
