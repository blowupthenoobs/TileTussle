using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour
{
    private SpriteRenderer image;
    public bool occupied;
    
    void Awake()
    {
        image = gameObject.GetComponent<SpriteRenderer>();
    }
}
