using System.Collections;
using System.Collections.Generic;
using MapStuff;
using UnityEngine;
using UnityEngine.Events;

public class TileScript : MonoBehaviour
{
    private SpriteRenderer image;
    public TileType tileType;
    public GameObject heldItem;

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

        if(type == TileType.playerGrass)
            ObtainItem(Instantiate(TileManagerScript.Instance.playerPrefab, transform.position, transform.rotation));
        if(type == TileType.enemyGrass)
            ObtainItem(Instantiate(TileManagerScript.Instance.enemyPrefab, transform.position, transform.rotation));
    }

    public void ObtainItem(GameObject item)
    {
        heldItem = item;
        item.transform.SetParent(transform);
    }

    void GetNeighbors()
    {
        leftTile = TileManagerScript.Instance.GetTile((int)transform.position.x - 1, (int)transform.position.y)?.GetComponent<TileScript>();
        bottomTile = TileManagerScript.Instance.GetTile((int)transform.position.x, (int)transform.position.y - 1)?.GetComponent<TileScript>();

        leftTile?.GiveRightNeighbor(this);
        bottomTile?.GiveTopNeighbor(this);
    }

    public void MakeBlue()
    {
        GetComponent<SpriteRenderer>().color = Color.blue;
    }

    public void GetTilesOfDistance(int distance)
    {
        if(tileType != TileType.empty)
        {
            MakeBlue();

            if(distance > 0)
            {
                rightTile.GetTilesOfDistance(distance - 1);
                topTile.GetTilesOfDistance(distance - 1);
                leftTile.GetTilesOfDistance(distance - 1);
                bottomTile.GetTilesOfDistance(distance - 1);    
            }
        }
    }


    public TileScript GetNeighborOfDirection(Vector2 direction)
    {
        if(direction == Vector2.up)
            return topTile;
        if(direction == Vector2.down)
            return bottomTile;
        if(direction == Vector2.left)
            return leftTile;
        if(direction == Vector2.right)
            return rightTile;
        
        Debug.Log("no tile for following direction");
        Debug.Log(direction);
        return this;    
    }

    public void GiveRightNeighbor(TileScript neighbor)
    {
        rightTile = neighbor;
    }

    public void GiveTopNeighbor(TileScript neighbor)
    {
        topTile = neighbor;
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
            case TileType.playerGrass:
                return Color.green;
            case TileType.enemyGrass:
                return Color.green;
            default:
                return Color.magenta;
        }
    }
}
