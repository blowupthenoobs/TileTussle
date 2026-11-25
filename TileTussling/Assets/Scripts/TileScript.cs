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
    }
}
