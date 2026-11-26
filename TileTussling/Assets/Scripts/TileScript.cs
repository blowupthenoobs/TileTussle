using System.Collections;
using System.Collections.Generic;
using MapStuff;
using UnityEngine;
using UnityEngine.Events;

public class TileScript : MonoBehaviour
{
    private SpriteRenderer image;
    public bool occupied;
    public TileType tileType;

    public TileScript topTile, rightTile, bottomTile, leftTile;
    public static UnityAction clearTileEffects;
    
    void Awake()
    {
        image = gameObject.GetComponent<SpriteRenderer>();
    }

    public void Spawn(TileType type)
    {
        tileType = type;
        GetComponent<SpriteRenderer>().color = GetTileColor(type);
        GetNeighbors();
        clearTileEffects += ResetTileColor;
    }

    void GetNeighbors()
    {
        leftTile = TileManagerScript.Instance.GetTile((int)transform.position.x - 1, (int)transform.position.y)?.GetComponent<TileScript>();
        bottomTile = TileManagerScript.Instance.GetTile((int)transform.position.x, (int)transform.position.y - 1)?.GetComponent<TileScript>();

        leftTile?.GiveRightNeighbor(this);
        bottomTile?.GiveTopNeighbor(this);
    }

    public void GiveRightNeighbor(TileScript neighbor)
    {
        rightTile = neighbor;
    }

    public void GiveTopNeighbor(TileScript neighbor)
    {
        topTile = neighbor;
    }

    public void MakeBlue()
    {
        GetComponent<SpriteRenderer>().color = Color.blue;
    }

    private void ResetTileColor()
    {
        GetComponent<SpriteRenderer>().color = GetTileColor(tileType);
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
