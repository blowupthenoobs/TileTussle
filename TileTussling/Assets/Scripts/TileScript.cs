using System.Collections;
using System.Collections.Generic;
using MapStuff;
using UnityEngine;

public class TileScript : MonoBehaviour
{
    private SpriteRenderer image;
    public bool occupied;
    public TileType tileType;
    
    void Awake()
    {
        image = gameObject.GetComponent<SpriteRenderer>();
    }

    public void Spawn(TileType type)
    {
        tileType = type;
        GetComponent<SpriteRenderer>().color = GetTileColor(type);
    }
    public Color GetTileColor(TileType type)
    {
        switch(type)
        {
            case TileType.empty:
                return new Color(64f/255f, 64f/255f, 64f/255f, 1);
            case TileType.grass:
                return Color.green;
            case TileType.sand:
                return Color.yellow;
            default:
                return Color.magenta;
        }
    }
}
